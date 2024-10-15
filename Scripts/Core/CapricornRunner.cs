using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Events;

namespace Dunward.Capricorn
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(CapricornDialogue))]
    [RequireComponent(typeof(CapricornSettings))]
    [RequireComponent(typeof(CapricornCache))]
    public partial class CapricornRunner : MonoBehaviour
    {
#region Unity Inspector Fields
        public bool isDebug;
#endregion

        private CapricornDialogue dialogue;
        private CapricornSettings settings;
        private CapricornCache cache;

        private GraphData graphData;

        internal Dictionary<int, NodeMainData> nodes = new Dictionary<int, NodeMainData>();

        internal UnityEvent bindingInteraction;
        public Func<List<string>, List<UnityEngine.UI.Button>> onSelectionCreate;

        public event CoroutineDelegate AddCustomCoroutines;
        public delegate IEnumerator CoroutineDelegate(CoroutineUnit unit);

        public float selectionDestroyAfterDelay = 1f;

        private int nextNodeIndex = -1;

        private Dictionary<string, GameObject> characters = new Dictionary<string, GameObject>();

        public NodeMainData StartNode
        {
            get => graphData.nodes.Find(node => node.nodeType == NodeType.Input);
        }

        public NodeMainData DebugNode
        {
            get => graphData.nodes.Find(node => node.id == graphData.debugNodeIndex);
        }

        private void Initialize()
        {
            dialogue = GetComponent<CapricornDialogue>();
            settings = GetComponent<CapricornSettings>();
            cache = GetComponent<CapricornCache>();

            dialogue.Initialize();
        }

        public void Load(string json)
        {
            Initialize();

            graphData = JsonConvert.DeserializeObject<GraphData>(json, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto
            });

            nodes.Clear();

            foreach (var node in graphData.nodes)
            {
                nodes.Add(node.id, node);
            }
        }

        public void Clear()
        {
            dialogue.NameTarget.SetText("");
            dialogue.SubNameTarget.SetText("");
            dialogue.ScriptTarget.SetText("");

            Destroy(cache.lastBackground);
            Destroy(cache.lastForeground);
            Destroy(cache.bgmObject);

            foreach (var character in characters)
            {
                Destroy(character.Value);
            }

            characters.Clear();
        }

        public IEnumerator Run()
        {
            var currentNode = isDebug ? DebugNode : StartNode;

            while (true)
            {
                yield return RunCoroutine(currentNode.coroutineData);

                yield return RunAction(currentNode.actionData.action);

                if (currentNode.nodeType == NodeType.Output) yield break;
                currentNode = Next();
            }
        }

        public IEnumerator RunFromID(int id)
        {
            var currentNode = graphData.nodes.Find(node => node.id == id);

            while (true)
            {
                yield return RunCoroutine(currentNode.coroutineData);

                yield return RunAction(currentNode.actionData.action);

                if (currentNode.nodeType == NodeType.Output) yield break;
                currentNode = Next();
            }
        }

        private IEnumerator RunCoroutine(NodeCoroutineData data)
        {
            foreach (var coroutine in data.coroutines)
            {
                if (coroutine.isWaitingUntilFinish)
                {
                    yield return StartCoroutine(ExecuteCoroutine(coroutine));
                }
                else
                {
                    StartCoroutine(ExecuteCoroutine(coroutine));
                }
            }
        }

        private IEnumerator ExecuteCoroutine(CoroutineUnit unit)
        {
            switch (unit)
            {
                case WaitUnit waitUnit:
                    yield return waitUnit.Execute();
                    break;
                case ShowCharacterUnit showCharacterUnit:
                    yield return showCharacterUnit.Execute(settings.characterArea, characters);
                    break;
                case ChangeBackgroundUnit changeBackgroundUnit:
                    yield return changeBackgroundUnit.Execute(settings.backgroundArea, cache.lastBackground);
                    break;
                case ChangeForegroundUnit changeForegroundUnit:
                    yield return changeForegroundUnit.Execute(settings.foregroundArea, cache.lastForeground);
                    break;
                case DeleteForegroundUnit deleteForegroundUnit:
                    yield return deleteForegroundUnit.Execute(cache.lastForeground);
                    break;
                case DeleteCharacterUnit deleteCharacterUnit:
                    yield return deleteCharacterUnit.Execute(characters);
                    break;
                case DeleteAllCharacterUnit deleteAllCharacterUnit:
                    yield return deleteAllCharacterUnit.Execute(characters);
                    break;
                case ClearDialogueTextUnit clearDialogueTextUnit:
                    yield return clearDialogueTextUnit.Execute(dialogue.NameTarget, dialogue.SubNameTarget, dialogue.ScriptTarget);
                    break;
                case PlayBGMUnit playBGMUnit:
                    yield return playBGMUnit.Execute(cache.bgmObject);
                    break;
                case StopBGMUnit stopBGMUnit:
                    yield return stopBGMUnit.Execute(cache.bgmObject);
                    break;
                case PlaySFXUnit playSFXUnit:
                    yield return playSFXUnit.Execute();
                    break;
                case TransformCharacterUnit transformCharacterUnit:
                    yield return transformCharacterUnit.Execute(characters);
                    break;
                default:
                    if (AddCustomCoroutines != null)
                    {
                        foreach (CoroutineDelegate coroutine in AddCustomCoroutines.GetInvocationList())
                        {
                            yield return coroutine(unit);
                        }
                    }
                    break;
            }
        }

        private IEnumerator RunAction(ActionUnit action)
        {
            nextNodeIndex = action.GetNextNodeIndex();
            switch (action)
            {
                case TextTypingUnit typing:
                    bindingInteraction.AddListener(typing.Interaction);
                    yield return typing.Execute(dialogue.NameTarget, dialogue.SubNameTarget, dialogue.ScriptTarget);
                    bindingInteraction.RemoveListener(typing.Interaction);
                    
                    break;
                case SelectionUnit selection:
                    var selections = onSelectionCreate.Invoke(selection.scripts);
                    yield return selection.Execute(selections, selectionDestroyAfterDelay);
                    nextNodeIndex = selection.GetNextNodeIndex();
                    break;
            }

        }

        private NodeMainData Next()
        {
            return nodes[nextNodeIndex];
        }
    }
}