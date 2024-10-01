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
            var currentNode = StartNode;

            while (true)
            {
                yield return RunCoroutine(currentNode.coroutineData);

                var action = CreateAction(currentNode.actionData);
                yield return RunAction(action);

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

        private IEnumerator RunAction(ActionPlayer action)
        {
            nextNodeIndex = action.GetNextNodeIndex();
            switch (action)
            {
                case TextDisplayer textDisplayer:
                    bindingInteraction.AddListener(textDisplayer.Interaction);
                    yield return textDisplayer.Execute(dialogue.NameTarget, dialogue.SubNameTarget, dialogue.ScriptTarget);
                    bindingInteraction.RemoveListener(textDisplayer.Interaction);
                    
                    break;
                case SelectionDisplayer selectionDisplayer:
                    var selections = onSelectionCreate.Invoke(selectionDisplayer.GetSelections());
                    yield return selectionDisplayer.Execute(selections, selectionDestroyAfterDelay);
                    nextNodeIndex = selectionDisplayer.GetNextNodeIndex();
                    break;
            }

        }

        private NodeMainData Next()
        {
            return nodes[nextNodeIndex];
        }
        
        private ActionPlayer CreateAction(NodeActionData actionData)
        {
            switch (actionData.actionNodeType)
            {
                case ActionType.CHARACTER:
                    return new TextDisplayer(actionData);
                case ActionType.USER:
                    return new SelectionDisplayer(actionData);
                default:
                    return new ActionPlayer(actionData);
            }
        }
    }
}