namespace QRCode.Engine.Toolbox.Pattern.StateMachine
{
    using System.Collections.Generic;
    using System.Linq;
    using QRCode.Engine.Debugging;
    using QRCode.Engine.Toolbox.Optimization;
    using QRCode.Engine.Toolbox.Tags;

    /// <summary>
    /// A finite state machine for sequential code structures.
    /// </summary>
    public class FiniteStateMachine : IDeletable
    {
        #region Fields
        #region Internals
        private Dictionary<int, State> _states = null;
        private Dictionary<State, List<State>> _links = null;
        private State _currentState = null;
        #endregion Internals
        #endregion Fields

        #region Constructor
        public FiniteStateMachine()
        {
            _states = new Dictionary<int, State>();
            _links = new Dictionary<State, List<State>>();
        }
        #endregion Constructor

        #region Methods
        #region LifeCycle
        public void Delete()
        {
            if (_states != null)
            {
                int stateCount = _states.Count;
                for (int i = 0; i < stateCount; i++)
                {
                    _states.Values.ElementAt(i).Delete();
                }
                
                _states.Clear();
                _states = null;
            }

            if (_links != null)
            {
                _links.Clear();
                _links = null;
            }

            _currentState = null;
        }
        #endregion LifeCycle

        #region Public Methods
        /// <summary>
        /// Start state machine and enter in an initial state.
        /// </summary>
        public void StartStateMachine(int initialState)
        {
            ChangeState(initialState);
        }

        /// <summary>
        /// Try to change state if the current state posses a link to the target state.
        /// </summary>
        public bool TryChangeState(int targetStateId)
        {
            State target = GetState(targetStateId);
            
            if (IsExistingLinks(_currentState, target))
            {
                ChangeState(targetStateId);
                return true;
            }
            
            QRLogger.DebugError<ToolboxTags.Patterns.FiniteStateMachine>($"Cannot find the link of the state {_currentState.Name} in {target.Name}.");
            return false;
        }

        /// <summary>
        /// Force the changement of a state and ignore the links.
        /// </summary>
        public void ForceChangeState(int stateId)
        {
            ChangeState(stateId);
        }
        
        /// <summary>
        /// Add a state to the state machine.
        /// </summary>
        public void AddState(State state)
        {
            _states.Add(state.Id, state);
            _links.Add(state, new List<State>());
        }

        /// <summary>
        /// Add a link between two state, with <see cref="TryChangeState"/>, you can only make transition between linked states.
        /// </summary>
        public void AddLink(State stateA, State stateB)
        {
            _links[stateA].Add(stateB);
        }
        
        /// <summary>
        /// Use this function to update the current state.
        /// </summary>
        public void UpdateStateMachine(float deltaTime)
        {
            if (_currentState != null)
            {
                _currentState.OnUpdate(deltaTime);
            }
        }
        #endregion Public Methods

        #region Private Methods
        private void ChangeState(int stateId)
        {
            State target = GetState(stateId);
            
            if (_currentState != null)
            {
                _currentState.OnExit();
            }

            _currentState = target;
            _currentState.OnEnter();
        }
        
        private bool IsExistingLinks(State a, State b)
        {
            return _links[a].Contains(b);
        }
        
        private State GetState(int id)
        {
            return _states[id];
        }
        #endregion Private Methods
        #endregion Methods
    }
}
