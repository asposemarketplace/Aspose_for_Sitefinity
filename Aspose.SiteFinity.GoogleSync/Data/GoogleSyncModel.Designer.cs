﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.Data.EntityClient;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Serialization;

[assembly: EdmSchemaAttribute()]
namespace Aspose.GoogleSync.Data
{
    #region Contexts
    
    /// <summary>
    /// No Metadata Documentation available.
    /// </summary>
    public partial class GoogleSyncEntities : ObjectContext
    {
        #region Constructors
    
        /// <summary>
        /// Initializes a new GoogleSyncEntities object using the connection string found in the 'GoogleSyncEntities' section of the application configuration file.
        /// </summary>
        public GoogleSyncEntities() : base("name=GoogleSyncEntities", "GoogleSyncEntities")
        {
            OnContextCreated();
        }
    
        /// <summary>
        /// Initialize a new GoogleSyncEntities object.
        /// </summary>
        public GoogleSyncEntities(string connectionString) : base(connectionString, "GoogleSyncEntities")
        {
            OnContextCreated();
        }
    
        /// <summary>
        /// Initialize a new GoogleSyncEntities object.
        /// </summary>
        public GoogleSyncEntities(EntityConnection connection) : base(connection, "GoogleSyncEntities")
        {
            OnContextCreated();
        }
    
        #endregion
    
        #region Partial Methods
    
        partial void OnContextCreated();
    
        #endregion
    
        #region ObjectSet Properties
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        public ObjectSet<Aspose_GoogleSync_ServerDetails> Aspose_GoogleSync_ServerDetails
        {
            get
            {
                if ((_Aspose_GoogleSync_ServerDetails == null))
                {
                    _Aspose_GoogleSync_ServerDetails = base.CreateObjectSet<Aspose_GoogleSync_ServerDetails>("Aspose_GoogleSync_ServerDetails");
                }
                return _Aspose_GoogleSync_ServerDetails;
            }
        }
        private ObjectSet<Aspose_GoogleSync_ServerDetails> _Aspose_GoogleSync_ServerDetails;

        #endregion

        #region AddTo Methods
    
        /// <summary>
        /// Deprecated Method for adding a new object to the Aspose_GoogleSync_ServerDetails EntitySet. Consider using the .Add method of the associated ObjectSet&lt;T&gt; property instead.
        /// </summary>
        public void AddToAspose_GoogleSync_ServerDetails(Aspose_GoogleSync_ServerDetails aspose_GoogleSync_ServerDetails)
        {
            base.AddObject("Aspose_GoogleSync_ServerDetails", aspose_GoogleSync_ServerDetails);
        }

        #endregion

    }

    #endregion

    #region Entities
    
    /// <summary>
    /// No Metadata Documentation available.
    /// </summary>
    [EdmEntityTypeAttribute(NamespaceName="GoogleSyncModel", Name="Aspose_GoogleSync_ServerDetails")]
    [Serializable()]
    [DataContractAttribute(IsReference=true)]
    public partial class Aspose_GoogleSync_ServerDetails : EntityObject
    {
        #region Factory Method
    
        /// <summary>
        /// Create a new Aspose_GoogleSync_ServerDetails object.
        /// </summary>
        /// <param name="id">Initial value of the ID property.</param>
        public static Aspose_GoogleSync_ServerDetails CreateAspose_GoogleSync_ServerDetails(global::System.Int32 id)
        {
            Aspose_GoogleSync_ServerDetails aspose_GoogleSync_ServerDetails = new Aspose_GoogleSync_ServerDetails();
            aspose_GoogleSync_ServerDetails.ID = id;
            return aspose_GoogleSync_ServerDetails;
        }

        #endregion

        #region Primitive Properties
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=true, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.Int32 ID
        {
            get
            {
                return _ID;
            }
            set
            {
                if (_ID != value)
                {
                    OnIDChanging(value);
                    ReportPropertyChanging("ID");
                    _ID = StructuralObject.SetValidValue(value);
                    ReportPropertyChanged("ID");
                    OnIDChanged();
                }
            }
        }
        private global::System.Int32 _ID;
        partial void OnIDChanging(global::System.Int32 value);
        partial void OnIDChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public global::System.String UserID
        {
            get
            {
                return _UserID;
            }
            set
            {
                OnUserIDChanging(value);
                ReportPropertyChanging("UserID");
                _UserID = StructuralObject.SetValidValue(value, true);
                ReportPropertyChanged("UserID");
                OnUserIDChanged();
            }
        }
        private global::System.String _UserID;
        partial void OnUserIDChanging(global::System.String value);
        partial void OnUserIDChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public global::System.String Username
        {
            get
            {
                return _Username;
            }
            set
            {
                OnUsernameChanging(value);
                ReportPropertyChanging("Username");
                _Username = StructuralObject.SetValidValue(value, true);
                ReportPropertyChanged("Username");
                OnUsernameChanged();
            }
        }
        private global::System.String _Username;
        partial void OnUsernameChanging(global::System.String value);
        partial void OnUsernameChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public global::System.String Email
        {
            get
            {
                return _Email;
            }
            set
            {
                OnEmailChanging(value);
                ReportPropertyChanging("Email");
                _Email = StructuralObject.SetValidValue(value, true);
                ReportPropertyChanged("Email");
                OnEmailChanged();
            }
        }
        private global::System.String _Email;
        partial void OnEmailChanging(global::System.String value);
        partial void OnEmailChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public global::System.String Password
        {
            get
            {
                return _Password;
            }
            set
            {
                OnPasswordChanging(value);
                ReportPropertyChanging("Password");
                _Password = StructuralObject.SetValidValue(value, true);
                ReportPropertyChanged("Password");
                OnPasswordChanged();
            }
        }
        private global::System.String _Password;
        partial void OnPasswordChanging(global::System.String value);
        partial void OnPasswordChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public global::System.String ClientID
        {
            get
            {
                return _ClientID;
            }
            set
            {
                OnClientIDChanging(value);
                ReportPropertyChanging("ClientID");
                _ClientID = StructuralObject.SetValidValue(value, true);
                ReportPropertyChanged("ClientID");
                OnClientIDChanged();
            }
        }
        private global::System.String _ClientID;
        partial void OnClientIDChanging(global::System.String value);
        partial void OnClientIDChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public global::System.String ClientSecret
        {
            get
            {
                return _ClientSecret;
            }
            set
            {
                OnClientSecretChanging(value);
                ReportPropertyChanging("ClientSecret");
                _ClientSecret = StructuralObject.SetValidValue(value, true);
                ReportPropertyChanged("ClientSecret");
                OnClientSecretChanged();
            }
        }
        private global::System.String _ClientSecret;
        partial void OnClientSecretChanging(global::System.String value);
        partial void OnClientSecretChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public global::System.String RefreshToken
        {
            get
            {
                return _RefreshToken;
            }
            set
            {
                OnRefreshTokenChanging(value);
                ReportPropertyChanging("RefreshToken");
                _RefreshToken = StructuralObject.SetValidValue(value, true);
                ReportPropertyChanged("RefreshToken");
                OnRefreshTokenChanged();
            }
        }
        private global::System.String _RefreshToken;
        partial void OnRefreshTokenChanging(global::System.String value);
        partial void OnRefreshTokenChanged();

        #endregion

    
    }

    #endregion

    
}
