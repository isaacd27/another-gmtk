using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace NobunAtelier
{
    public class StateMachineComponent<T> : StateComponent<T>
        where T : StateDefinition
    {
        public T CurrentStateDefinition => m_activeStateDefinition;

        private Dictionary<T, StateComponent<T>> m_statesMap = new Dictionary<T, StateComponent<T>>();
        private T m_activeStateDefinition = null;

        [Header("--- State Machine Debug ---")]
        [SerializeField]
        protected bool m_displayDebug = false;
        
        protected override void Start()
        {
            base.Start();
        }

        public void RegisterStateComponent(StateComponent<T> state)
        {
            m_statesMap.Add(state.GetStateDefinition(), state);
        }

        public override void SetState(T newState)
        {
            if(newState == m_activeStateDefinition)
            {
                return;
            }

            if (!m_statesMap.ContainsKey(newState))
            {
                base.SetState(newState);
                return;
            }

            m_statesMap[m_activeStateDefinition].Exit();
            m_activeStateDefinition = newState;
            m_statesMap[m_activeStateDefinition].Enter();
        }

        public override void Enter()
        {
            if (m_stateDefinition != null)
            {
                m_activeStateDefinition = m_stateDefinition;
                while (m_activeStateDefinition.RequiredPriorState != null)
                {
                    Debug.LogWarning($"Required condition <b>{m_activeStateDefinition.RequiredPriorState.name}</b> for state <b>{m_activeStateDefinition.name}</b>. " +
                        $"Rolling back state to <b>{m_activeStateDefinition.RequiredPriorState.name}</b>.");
                    m_activeStateDefinition = m_activeStateDefinition.RequiredPriorState as T;
                }

                if (!m_statesMap.ContainsKey(m_activeStateDefinition))
                {
                    Debug.LogError($"State machine doesn't have a valid StateComponent for state <b>{m_activeStateDefinition.name}</b>");
                }
                m_statesMap[m_activeStateDefinition].Enter();
            }
        }

        public override void Exit()
        {
            if (m_activeStateDefinition != null)
            {
                m_statesMap[m_activeStateDefinition].Exit();
            }
        }

        public override void Tick(float deltaTime)
        {
            if (m_activeStateDefinition == null)
            {
                return;
            }

            m_statesMap[m_activeStateDefinition].Tick(deltaTime);
        }

        protected virtual void OnGUI()
        {
            if (!Application.isPlaying || !m_displayDebug)
            {
                return;
            }

            GUILayout.BeginVertical(GUI.skin.box);
            GUILayout.Label("<b>------------</b>");
            GUILayout.Label("<b>State Machine Behaviour</b>");
            GUILayout.Label($"<b>Current state: {m_activeStateDefinition.name}</b>");

            foreach(var a in m_statesMap)
            {
                UIDebugDrawLabelValue(a.Key.name, a.Value.name);                
            }

            GUILayout.EndVertical();
        }

        private void UIDebugDrawLabelValue(string label, string value)
        {
            GUILayout.BeginHorizontal();
            {
                GUILayout.Label(label + ":");
                GUILayout.Label(value);
            }
            GUILayout.EndHorizontal();
        }
    }
}