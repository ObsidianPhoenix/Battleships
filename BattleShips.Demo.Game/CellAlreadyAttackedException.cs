using System;
using System.Runtime.Serialization;

namespace BattleShips.Demo
{
    [Serializable]
    public class CellAlreadyAttackedException : ApplicationException
    {
        #region Public Constructors

        public CellAlreadyAttackedException()
        {
        }

        public CellAlreadyAttackedException(string message)
            : base(message)
        {
        }

        public CellAlreadyAttackedException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        #endregion Public Constructors

        #region Protected Constructors

        protected CellAlreadyAttackedException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        #endregion Protected Constructors
    }
}