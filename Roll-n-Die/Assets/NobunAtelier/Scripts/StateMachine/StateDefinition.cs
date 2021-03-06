using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NobunAtelier
{
    public abstract class StateDefinition : ScriptableObject
    {
        public StateDefinition RequiredPriorState => m_requiredPriorState;

        [SerializeField]
        private StateDefinition m_requiredPriorState;
    }
}

