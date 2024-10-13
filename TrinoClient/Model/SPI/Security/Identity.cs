using System;
using System.Text;

namespace TrinoClient.Model.SPI.Security
{
    /// <summary>
    /// From com.facebook.presto.spi.security.Identity.java
    /// </summary>
    public class Identity
    {
        #region Public Properties

        public string User { get; }

        /// <summary>
        /// TODO: This is a java.security.Principal object
        /// </summary>
        public dynamic Principal { get; }

        #endregion

        #region Constructors

        public Identity(string user, dynamic principal)
        {
            ParameterCheck.NotNullOrEmpty(user, "user");

            User = user;
            Principal = principal;
        }

        #endregion

        #region Public Methods

        public override bool Equals(object obj)
        {
            if (this == obj)
            {
                return true;
            }

            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            Identity Id = (Identity)obj;

            return Object.Equals(User, Id.User);
        }

        public override int GetHashCode()
        {
            return Hashing.Hash(User);
        }

        public override string ToString()
        {
            StringBuilder SB = new("Identity{");
            SB.Append("user='").Append(User).Append("\'");

            if (Principal != null)
            {
                SB.Append(", prinicipal="); //.Append(this.Principal.Get());
            }

            SB.Append("}");

            return SB.ToString();
        }

        #endregion
    }
}
