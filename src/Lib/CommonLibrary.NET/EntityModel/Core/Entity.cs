/*
 * Author: Kishore Reddy
 * Url: http://commonlibrarynet.codeplex.com/
 * Title: CommonLibrary.NET
 * Copyright: � 2009 Kishore Reddy
 * License: LGPL License
 * LicenseUrl: http://commonlibrarynet.codeplex.com/license
 * Description: A C# based .NET 3.5 Open-Source collection of reusable components.
 * Usage: Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
using System;
using System.Data;
using System.Collections.Generic;

using ComLib;
using ComLib.ValidationSupport;


namespace ComLib.Entities
{
    
    /// <summary>
    /// Persistant entity that is auditable and can be validated.
    /// </summary>
    public abstract class Entity : EntityPersistantAudtiable<int>, IEntity, ICloneable
    {
        /// <summary>
        /// The results after validation.
        /// </summary>
        protected IValidationResults _validationResults = new ValidationResults();


        /// <summary>
        /// Settings specific for the entity.
        /// e.g. MaxLengthOfTitle = 10 etc.
        /// </summary>
        public IEntitySettings Settings { get; set; }


        #region Validation
        /// <summary>
        /// Validate this entity.
        /// </summary>
        /// <returns></returns>
        public virtual bool Validate()
        {
            _validationResults.Clear();
            Validate(_validationResults);
            return _validationResults.IsValid;
        }


        /// <summary>
        /// Validate this entity and collects/stores any validation errors
        /// into the results supplied.
        /// </summary>
        /// <returns></returns>
        public virtual bool Validate(IValidationResults results)
        {
            var validator = GetValidator();

            // Life-cycle callbacks before validation.
            OnBeforeValidate(null);

            // Do not collect errors into this instance.
            bool isValid = validator.Validate(this, results);

            // Life-cycle callbacks
            if(isValid) OnAfterValidate();

            return isValid;
        }


        /// <summary>
        /// Validate this entity and collects/stores any validation errors
        /// into the results supplied and optionally copies the errors into the internal error state for this entity.
        /// </summary>
        /// <returns></returns>
        public virtual bool Validate(IValidationResults results, bool copyToInternalErrors)
        {
            if (!copyToInternalErrors) return Validate(results);

            Validate();
            _validationResults.CopyTo(results);
            return _validationResults.IsValid;
        }


        /// <summary>
        /// Validates this object.
        /// Only difference compared to Validate() is this more convenient.
        /// and yeilds a bool instead of the actual validation results.
        /// </summary>
        public bool IsValid
        {
            get
            {
                return _validationResults.IsValid;
            }
        }


        /// <summary>
        /// Validation errors.
        /// </summary>
        public IValidationResults Errors
        {
            get { return _validationResults; }
        }


        /// <summary>
        /// Gets the validator for this entity.
        /// </summary>
        /// <returns></returns>
        protected virtual IValidator GetValidator()
        {
            if (EntityRegistration.HasValidator(GetType()))
            {
                var validator = EntityRegistration.GetValidator(GetType());
                return validator as IEntityValidator;
            }
            return GetValidatorInternal();
        }


        /// <summary>
        /// This is here to facilitate code-generation so that this method can
        /// be overriden in the generated code. But that generated code
        /// can then be overriden by the user for customgenerated code by 
        /// over-riding the GetValidator().
        /// e.g. GetValidatorInternal() => codegenerator overrides.
        ///      GetValidator()         => user overrides in separate partial class
        ///                                to not use the code generation
        /// </summary>
        /// <returns></returns>
        protected virtual IValidator GetValidatorInternal()
        {
            return Validator.Empty;
        }
        #endregion


        #region Equality
        /// <summary>
        /// Comapare this object with <paramref name="obj"/>
        /// </summary>
        /// <param name="obj">Object to compare</param>
        /// <returns>True if the two objects are equal, false otherwise</returns>
        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (obj.GetType() != this.GetType()) return false;

            return ((Entity)obj).Id == this.Id;
        }


        /// <summary>
        /// Gets the hashcode to uniquely represent the entity. Compliments the Equals method
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            // NOTE: Need to consider the objects type?
            return this.Id.GetHashCode();
        }


        /// <summary>
        /// Check if 2 entities are the same.
        /// </summary>
        public static bool operator ==(Entity first, Entity second)
        {
            if (Object.ReferenceEquals(first, second)) return true;
            if ((object)first == null || (object)second == null) return false;

            return first.Id == second.Id;
        }


        /// <summary>
        /// Checks if 2 entities are different.
        /// </summary>
        public static bool operator !=(Entity first, Entity second)
        {
            return !(first == second);
        }
        #endregion        
    

        #region ICloneable
        /// <summary>
        /// Clones this instance
        /// </summary>
        /// <returns></returns>
        public virtual object Clone()
        {
            return this.MemberwiseClone();
        }
        #endregion


        #region Callbacks
        /// <summary>
        /// Called when [before validate].
        /// </summary>
        /// <param name="ctx">The context information.</param>
        /// <returns></returns>
        public virtual bool OnBeforeValidate(object ctx)
        {
            return true;
        }


        /// <summary>
        /// Called when [before validate create].
        /// </summary>
        public virtual bool OnBeforeValidateCreate(object ctx)
        {
            return true;
        }


        /// <summary>
        /// Called when [before validate update].
        /// </summary>
        public virtual bool OnBeforeValidateUpdate(object ctx)
        {
            return true;
        }


        /// <summary>
        /// Called when [before create].
        /// </summary>
        public virtual bool OnBeforeCreate(object ctx)
        {
            return true;
        }


        /// <summary>
        /// Called when [before update].
        /// </summary>
        public virtual bool OnBeforeUpdate(object ctx)
        {
            return true;
        }


        /// <summary>
        /// Called when [before save].
        /// </summary>
        public virtual bool OnBeforeSave(object ctx)
        {
            return true;
        }


        /// <summary>
        /// Called when [before delete].
        /// </summary>
        public virtual bool OnBeforeDelete(object ctx)
        {
            return true;
        }


        /// <summary>
        /// Called after call to Class.New().
        /// </summary>
        public virtual void OnAfterNew()
        {
        }


        /// <summary>
        /// Called when [after validate].
        /// </summary>
        public virtual void OnAfterValidate()
        {
        }


        /// <summary>
        /// Called when [after validate create].
        /// </summary>
        public virtual void OnAfterValidateCreate()
        {
        }


        /// <summary>
        /// Called when [after validate update].
        /// </summary>
        public virtual void OnAfterValidateUpdate()
        {
        }


        /// <summary>
        /// Called when [after create].
        /// </summary>
        public virtual void OnAfterCreate()
        {
        }


        /// <summary>
        /// Called when [after update].
        /// </summary>
        public virtual void OnAfterUpdate()
        {
        }


        /// <summary>
        /// Called when [after save].
        /// </summary>
        public virtual void OnAfterSave()
        {
        }


        /// <summary>
        /// Called when [after delete].
        /// </summary>
        public virtual void OnAfterDelete()
        {
        }

        #endregion
    }

}
