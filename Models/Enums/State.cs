using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Models.Enums
{
    public enum State
    {
        Active,
        Removed,
        Blocked,

        #region For Movements
        Payment,
        All,
        #endregion

        #region FOR ALERTS
        View,
        #endregion

        #region FOR LOANS
        Reclosing,
        #endregion
        #region FOR HistoryPaymentsLoan
        OnlyInterest,
        Cancelled
        #endregion
    }
}
