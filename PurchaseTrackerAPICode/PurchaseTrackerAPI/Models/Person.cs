using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PurchaseTrackerAPI.Models
{
    public class Person
    {
        #region Declarations
        Int32 idPerson;
        Int32 salutationId;
        Int32 mobileNo;
        Int32 alternateMobNo;
        Int32 phoneNo;
        Int32 createdBy;
        DateTime dateOfBirth;
        DateTime createdOn;
        String firstName;
        String midName;
        String lastName;
        String primaryEmail;
        String alternateEmail;
        String comments;
        #endregion

        #region Constructor
        public Person()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdPerson
        {
            get { return idPerson; }
            set { idPerson = value; }
        }
        public Int32 SalutationId
        {
            get { return salutationId; }
            set { salutationId = value; }
        }
        public Int32 MobileNo
        {
            get { return mobileNo; }
            set { mobileNo = value; }
        }
        public Int32 AlternateMobNo
        {
            get { return alternateMobNo; }
            set { alternateMobNo = value; }
        }
        public Int32 PhoneNo
        {
            get { return phoneNo; }
            set { phoneNo = value; }
        }
        public Int32 CreatedBy
        {
            get { return createdBy; }
            set { createdBy = value; }
        }
        public DateTime DateOfBirth
        {
            get { return dateOfBirth; }
            set { dateOfBirth = value; }
        }
        public DateTime CreatedOn
        {
            get { return createdOn; }
            set { createdOn = value; }
        }
        public String FirstName
        {
            get { return firstName; }
            set { firstName = value; }
        }
        public String MidName
        {
            get { return midName; }
            set { midName = value; }
        }
        public String LastName
        {
            get { return lastName; }
            set { lastName = value; }
        }
        public String PrimaryEmail
        {
            get { return primaryEmail; }
            set { primaryEmail = value; }
        }
        public String AlternateEmail
        {
            get { return alternateEmail; }
            set { alternateEmail = value; }
        }
        public String Comments
        {
            get { return comments; }
            set { comments = value; }
        }
        #endregion
    }
}
