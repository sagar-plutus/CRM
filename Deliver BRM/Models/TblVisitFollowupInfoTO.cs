using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesTrackerAPI.Models
{
    public class TblVisitFollowupInfoTO
    {
        #region Declarations
        Int32 idVisitFollowUpInfo;
        Int32 visitId;
        Int32 shareInfoToWhom;
        Int32 shareInfoToWhomId;
        Int32 callBySelfToWhom;
        Int32 callBySelfToWhomId;
        Int32 callBySelfToWhomContactNo;
        Int32 arrangeVisitOf;
        Int32 arrangeVisitFor;
        Int32 createdBy;
        Int32 updatedBy;
        DateTime shareInfoOn;
        DateTime callBySelfOn;
        DateTime arrangeVisitOn;
        DateTime arrangeVisitReminder;
        DateTime influencerReminder;
        DateTime createdOn;
        DateTime updatedOn;

        Int32 possiblityConversionId;
        Int32 possibilityQualitySatisfactionId;
        Int32 possibilityServiceSatisfactionId;

        Int32 arrangeVisitTo;
        List<DropDownTO> visitFollowUpInfoNewList;//Sudhir[20-NOV-2017] Added for NewEntry of FollowUpRoles.
        Int32 visitCallBySelfToIsNew;//Sudhir [23-NOV-2017] Added for SettingFlag isNewUser
        String visitCallBySelfToName; //Sudhir[23 - NOV - 2017] Added for NewUserName
        String visitCallBySelfToMobNo; //Sudhir[23 - NOV - 2017] Added for NewContactNo.
        #endregion

        #region Constructor
        public TblVisitFollowupInfoTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdProjectFollowUpInfo
        {
            get { return idVisitFollowUpInfo; }
            set { idVisitFollowUpInfo = value; }
        }

        public Int32 VisitId
        {
            get { return visitId; }
            set { visitId = value; }
        }
        public Int32 ShareInfoToWhom
        {
            get { return shareInfoToWhom; }
            set { shareInfoToWhom = value; }
        }
        public Int32 ShareInfoToWhomId
        {
            get { return shareInfoToWhomId; }
            set { shareInfoToWhomId = value; }
        }

        public Int32 CallBySelfToWhom
        {
            get { return callBySelfToWhom; }
            set { callBySelfToWhom = value; }
        }

        public Int32 CallBySelfToWhomId
        {
            get { return callBySelfToWhomId; }
            set { callBySelfToWhomId = value; }
        }

        public Int32 CallBySelfToWhomContactNo
        {
            get { return callBySelfToWhomContactNo; }
            set { callBySelfToWhomContactNo = value; }
        }
        public Int32 ArrangeVisitOf
        {
            get { return arrangeVisitOf; }
            set { arrangeVisitOf = value; }
        }
        public Int32 ArrangeVisitFor
        {
            get { return arrangeVisitFor; }
            set { arrangeVisitFor = value; }
        }
        public Int32 CreatedBy
        {
            get { return createdBy; }
            set { createdBy = value; }
        }
        public Int32 UpdatedBy
        {
            get { return updatedBy; }
            set { updatedBy = value; }
        }
        public DateTime ShareInfoOn
        {
            get { return shareInfoOn; }
            set { shareInfoOn = value; }
        }
        public DateTime CallBySelfOn
        {
            get { return callBySelfOn; }
            set { callBySelfOn = value; }
        }
        public DateTime ArrangeVisitOn
        {
            get { return arrangeVisitOn; }
            set { arrangeVisitOn = value; }
        }
        public DateTime ArrangeVisitReminder
        {
            get { return arrangeVisitReminder; }
            set { arrangeVisitReminder = value; }
        }
        public DateTime InfluencerReminder
        {
            get { return influencerReminder; }
            set { influencerReminder = value; }
        }
        public DateTime CreatedOn
        {
            get { return createdOn; }
            set { createdOn = value; }
        }
        public DateTime UpdatedOn
        {
            get { return updatedOn; }
            set { updatedOn = value; }
        }
        public Int32 ArrangeVisitTo
        {
            get { return arrangeVisitTo; }
            set { arrangeVisitTo = value; }
        }
        public Int32 PossiblityConversionId
        {
            get { return possiblityConversionId; }
            set { possiblityConversionId = value; }
        }
        public Int32 PossibilityQualitySatisfactionId
        {
            get { return possibilityQualitySatisfactionId; }
            set { possibilityQualitySatisfactionId = value; }
        }
        public Int32 PossibilityServiceSatisfactionId
        {
            get { return possibilityServiceSatisfactionId; }
            set { possibilityServiceSatisfactionId = value; }
        }
        public List<DropDownTO> VisitFollowUpInfoNewList
        {
            get { return visitFollowUpInfoNewList; }
            set { visitFollowUpInfoNewList = value; }
        }
        public Int32 VisitCallBySelfToIsNew
        {
            get { return visitCallBySelfToIsNew; }
            set { visitCallBySelfToIsNew = value; }
        }

        public String VisitCallBySelfToMobNo
        {
            get { return visitCallBySelfToMobNo; }
            set { visitCallBySelfToMobNo = value; }
        }
        public String VisitCallBySelfToName
        {
            get { return visitCallBySelfToName; }
            set { visitCallBySelfToName = value; }
        }
        

        #endregion
    }
}
