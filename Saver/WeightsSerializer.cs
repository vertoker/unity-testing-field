using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace NN.Saver
{
    [CustomEditor(typeof(NeuralNetworkSaverUnity))]
    public class WeightsSerializer : Editor
    {
        private bool editTopology;
        private bool attention;
        
        private List<bool> showWeights;
        private List<int> topology;

        public override VisualElement CreateInspectorGUI()
        {
            var data = target as NeuralNetworkSaverUnity;
            
            editTopology = false;
            attention = false;
            showWeights = new bool[data.editorTopology.Length - 1].ToList();
            topology = data.editorTopology.ToList();
            
            return base.CreateInspectorGUI();
        }

        public override void OnInspectorGUI()
        {
            var data = target as NeuralNetworkSaverUnity;
            EditorGUILayout.Space();
            
            if (GUILayout.Button("Fill weights"))
            {
                attention = !attention;
            }
            if (attention)
            {
                EditorGUILayout.LabelField($"If you have data - create backup of it");
                EditorGUILayout.LabelField($"Are you sure want to fill weights by zero's");
                
                if (GUILayout.Button("Yes, Fill weights data by zero's"))
                {
                    attention = false;
                    data.FillEmpty();
                    showWeights = new bool[data.editorTopology.Length - 1].ToList();
                    EditorUtility.SetDirty(data);
                    AssetDatabase.SaveAssets();
                }
            }
            
            EditorGUILayout.Space(30);
            
            EditorGUILayout.LabelField($"ATTENTION: be accurate with editing topology");
            EditorGUILayout.LabelField($"Data about weights may be corrupted");
            EditorGUILayout.LabelField($"If you want create a new NN, better do it through the code");
            EditorGUILayout.LabelField($"AFTER EDITING press 'Fill weights' button");
            
            EditorGUILayout.Space(10);

            editTopology = EditorGUILayout.Toggle($"Edit topology", editTopology);
            
            int topologyCount = topology.Count;
            if (editTopology)
            {
                for (int i = 0; i < topologyCount; i++)
                {
                    EditorGUILayout.BeginHorizontal();

                    var temp = topology[i];
                    topology[i] = EditorGUILayout.IntField($"Neuron layer #{i+1}", Mathf.Clamp(topology[i], 1, 5000));
                    
                    if (GUILayout.Button("+"))
                    {
                        topology.Insert(i, topology[i]);
                        data.editorTopology = topology.ToArray();
                        EditorUtility.SetDirty(data);
                        return;
                    }
                    if (topologyCount > 2)
                    {
                        if (GUILayout.Button("x"))
                        {
                            topology.RemoveAt(i);
                            data.editorTopology = topology.ToArray();
                            EditorUtility.SetDirty(data);
                            return;
                        }
                    }
                    if (topologyCount > 1)
                    {
                        if (GUILayout.Button("v") && i < topologyCount - 1)
                        {
                            (topology[i], topology[i + 1]) = (topology[i + 1], topology[i]);
                            data.editorTopology = topology.ToArray();
                            EditorUtility.SetDirty(data);
                            return;
                        }
                        if (GUILayout.Button("^") && i > 0)
                        {
                            (topology[i - 1], topology[i]) = (topology[i], topology[i - 1]);
                            data.editorTopology = topology.ToArray();
                            EditorUtility.SetDirty(data);
                            return;
                        }
                    }
                
                    EditorGUILayout.EndHorizontal();

                    if (temp != topology[i])
                    {
                        data.editorTopology = topology.ToArray();
                        EditorUtility.SetDirty(data);
                    }
                }
            }
            else
            {
                for (int i = 0; i < topologyCount; i++)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField($"Neuron layer #{i+1}");
                    EditorGUILayout.LabelField(topology[i].ToString(CultureInfo.InvariantCulture));
                    EditorGUILayout.EndHorizontal();
                }
            }

            EditorGUILayout.Space(30);
            
            var counter = 0;
            int weightsCount = data.topology.Length - 1;
            for (int c = 0; c < weightsCount; c++)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField($"Weights Layer #{c+1}");
                showWeights[c] = EditorGUILayout.Toggle($"Show weights", showWeights[c]);
                EditorGUILayout.EndHorizontal();

                int inputCount = data.topology[c] + 1;
                int outputCount = data.topology[c + 1];

                if (showWeights[c])
                {
                    for (int o = 0; o < outputCount; o++)
                    {
                        var str = new StringBuilder(data.topology[c] * 10 + 4);
                        str.Append($"O{o+1}: ");
                    
                        for (int i = 0; i < inputCount; i++)
                        {
                            str.Append($"{data.weights[counter++].ToString(CultureInfo.InvariantCulture)} ");
                        }
                    
                        EditorGUILayout.LabelField(str.ToString());
                    }
                }
                else
                {
                    counter += inputCount * outputCount;
                }
            }
        }
    }
}
