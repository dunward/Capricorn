using System;

using UnityEngine;

using Newtonsoft.Json;
using System.Collections.Generic;

namespace Dunward.Capricorn
{
    public class CapricornBuilder
    {
        private CapricornRunner runner;

        private bool isNameTargetsSet = false;
        private bool isCharacterAreaSet = false;
        private bool isBackgroundAreaSet = false;
        private bool isForegroundAreaSet = false;

        private CapricornBuilder()
        {
            runner = new CapricornRunner();
        }

        public static CapricornBuilder Load(string text)
        {
            var builder = new CapricornBuilder();

            builder.runner.graphData = JsonConvert.DeserializeObject<GraphData>(text, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto
            });

            foreach (var node in builder.runner.graphData.nodes)
            {
                builder.runner.nodes.Add(node.id, node);
            }

            return builder;
        }

        public CapricornBuilder SetNameTargets(TMPro.TMP_Text nameTarget, TMPro.TMP_Text subNameTarget, TMPro.TMP_Text scriptTarget)
        {
            runner.nameTarget = nameTarget;
            runner.subNameTarget = subNameTarget;
            runner.scriptTarget = scriptTarget;

            isNameTargetsSet = true;

            return this;
        }

        public CapricornBuilder SetNameTargets(UnityEngine.UI.Text nameTarget, UnityEngine.UI.Text subNameTarget, UnityEngine.UI.Text scriptTarget)
        {
            runner.nameTarget = nameTarget;
            runner.subNameTarget = subNameTarget;
            runner.scriptTarget = scriptTarget;

            isNameTargetsSet = true;

            return this;
        }

        public CapricornBuilder SetCharacterArea(Transform characterArea)
        {
            runner.characterArea = characterArea;

            isCharacterAreaSet = true;

            return this;
        }

        public CapricornBuilder SetBackgroundArea(Transform backgroundArea)
        {
            runner.backgroundArea = backgroundArea;

            isBackgroundAreaSet = true;

            return this;
        }

        public CapricornBuilder SetForegroundArea(Transform foregroundArea)
        {
            runner.foregroundArea = foregroundArea;

            isForegroundAreaSet = true;

            return this;
        }

        public CapricornBuilder SetInteraction(UnityEngine.Events.UnityEvent evt)
        {
            evt.AddListener(() => runner.onInteraction?.Invoke());

            return this;
        }

        public CapricornRunner Build(MonoBehaviour target)
        {
            if (!isNameTargetsSet)
                throw new InvalidOperationException("Name targets are not set.");
            if (!isCharacterAreaSet)
                throw new InvalidOperationException("Character area is not set.");
            if (!isBackgroundAreaSet)
                throw new InvalidOperationException("Background area is not set.");
            if (!isForegroundAreaSet)
                throw new InvalidOperationException("Foreground area is not set.");

            runner.target = target;

            return runner;
        }
    }
}