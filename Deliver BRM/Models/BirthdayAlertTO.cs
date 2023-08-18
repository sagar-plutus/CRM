using System;
using System.Collections.Generic;
using System.Text;

namespace SalesTrackerAPI.Models
{
    public class BirthdayAlertTO {
        #region Declarations
        Int32 idPerson;
        Int32 salutationId;
        String mobileNo;
        String alternateMobNo;
        String phoneNo;
        Int32 createdBy;
        DateTime dateOfBirth;
        DateTime createdOn;
        String firstName;
        String midName;
        String lastName;
        String primaryEmail;
        String alternateEmail;
        String comments;
        Int32 seqNo;
        String salutationName;
        String photoBase64;
        Int32 dobDay;
        Int32 dobMonth;
        Int32 dobYear;

        Int32 anniDay;

        Int32 anniMonth;
        Int32 anniYear;
        DateTime dateOfAnniversary;
        Int32 otherDesignationId;
        Int32 idOrganization;
        String firmName;
        String website;
        Int32 idPersonType;
        String personType;

        Int32 idRole;

        String roleDesc;

        #endregion

        #region Constructor
        public BirthdayAlertTO () { }

        #endregion

        #region GetSet
        public Int32 IdPerson {
            get { return idPerson; }
            set { idPerson = value; }
        }
        public Int32 SalutationId {
            get { return salutationId; }
            set { salutationId = value; }
        }
        public String MobileNo {
            get { return mobileNo; }
            set { mobileNo = value; }
        }
        public String AlternateMobNo {
            get { return alternateMobNo; }
            set { alternateMobNo = value; }
        }
        public String PhoneNo {
            get { return phoneNo; }
            set { phoneNo = value; }
        }
        public Int32 CreatedBy {
            get { return createdBy; }
            set { createdBy = value; }
        }
        public DateTime DateOfBirth {
            get { return dateOfBirth; }
            set { dateOfBirth = value; }
        }
        public DateTime CreatedOn {
            get { return createdOn; }
            set { createdOn = value; }
        }
        public String FirstName {
            get { return firstName; }
            set { firstName = value; }
        }
        public String MidName {
            get { return midName; }
            set { midName = value; }
        }
        public String LastName {
            get { return lastName; }
            set { lastName = value; }
        }
        public String PrimaryEmail {
            get { return primaryEmail; }
            set { primaryEmail = value; }
        }
        public String AlternateEmail {
            get { return alternateEmail; }
            set { alternateEmail = value; }
        }
        public String Comments {
            get { return comments; }
            set { comments = value; }
        }

        public Int32 SeqNo {
            get { return seqNo; }
            set { seqNo = value; }
        }

        public string SalutationName { get => salutationName; set => salutationName = value; }
        public string PhotoBase64 { get => photoBase64; set => photoBase64 = value; }
        public String DateOfBirthStr {
            get {
                if (dateOfBirth != DateTime.MinValue)
                    return dateOfBirth.ToString ("dd-MM-yyyy");
                else return string.Empty;
            }
        }

        public int DobDay { get => dobDay; set => dobDay = value; }
        public int DobMonth { get => dobMonth; set => dobMonth = value; }
        public int DobYear { get => dobYear; set => dobYear = value; }

        public int AnniDay { get => anniDay; set => anniDay = value; }
        public int AnniMonth { get => anniMonth; set => anniMonth = value; }
        public int AnniYear { get => anniYear; set => anniYear = value; }
        public DateTime DateOfAnniversary { get => dateOfAnniversary; set => dateOfAnniversary = value; }
        public int OtherDesignationId { get => otherDesignationId; set => otherDesignationId = value; }

        public Int32 IdOrganization {
            get { return idOrganization; }
            set { idOrganization = value; }
        }

        public String FirmName {
            get { return firmName; }
            set { firmName = value; }
        }

        public string Website {
            get { return website; }

            set { website = value; }
        }

        public string PersonType {
            get { return personType; }

            set { personType = value; }
        }

        public Int32 IdPersonType {
            get { return idPersonType; }
            set { idPersonType = value; }
        }

        public Int32 IdRole {
            get { return idRole; }
            set { idRole = value; }
        }

        public string RoleDesc {
            get { return roleDesc; }

            set { roleDesc = value; }
        }
        #endregion
    }
}