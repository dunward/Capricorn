using System;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using UnityEngine;

namespace Dunward.Capricorn
{
    public class CapricornBuilder
    {
        private CapricornRunner runner;

        private bool isNameTargetsSet = false;

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

        public CapricornRunner Build(MonoBehaviour target)
        {
            if (!isNameTargetsSet)
                throw new InvalidOperationException("Name targets are not set.");

            runner.target = target;

            return runner;
        }
    }
}