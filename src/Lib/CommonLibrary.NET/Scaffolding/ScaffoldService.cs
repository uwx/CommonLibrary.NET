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
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Data;
using System.Reflection;
using ComLib;
using ComLib.Data;
using ComLib.Entities;
using ComLib.Entities.Management;
using ComLib.Reflection;


namespace ComLib.Scaffolding
{
    public class ScaffoldService
    {

            /// <summary>
            /// Settings for the entity service.
            /// </summary>
            public ScaffoldSettings Settings { get; set; }


            #region Public CRUD operations on the services related to the domain models.
            /// <summary>
            /// Update the specific entity with property values specified.
            /// </summary>
            /// <param name="entityName">"Category"</param>
            /// <param name="propValues">List of key value pairs.
            /// Where the key is the property name, and the value is a string 
            /// representation of the property value.</param>
            public BoolMessage Create(ScaffoldContext ctx)
            {
                BoolMessageItem creationResult = CreateContext(ctx, false, true, true);
                if (!creationResult.Success) return creationResult;

                EntityServiceContext entityActionContext = creationResult.Item as EntityServiceContext;

                // Create the entity using the entity service.
                EntityManager service = new EntityManager();
                BoolMessage result = service.Create(entityActionContext);
                return result;
            }


            /// <summary>
            /// Retrieve datatable containing all the records for the specified entity.
            /// </summary>
            /// <param name="entity"></param>
            /// <param name="clientId"></param>
            /// <returns></returns>
            public DataTable GetAll(ScaffoldContext ctx)
            {
                EntityManager entitySvc = new EntityManager();
                EntityServiceContext entityServiceContext = CreateContext(ctx, false, false, false).Item as EntityServiceContext;
                BoolMessageItem result = entitySvc.GetAll(entityServiceContext);
                if (!result.Success) { return null; }

                IList allItems = result.Item as IList;
                DataTable table = null;

                // Check for 0 results.
                if (allItems != null)
                {
                    // If we have results, convert each object in the list
                    // to a record, so we're effectively converting the object list
                    // to a DataTable.
                    if (allItems.Count > 0)
                    {
                        IList<PropertyInfo> properties = GetProperties(ctx.EntityName);
                        table = DataUtils.ConvertPropertyCollectionToDataTable(allItems, properties);
                    }
                }
                return table;
            }


            /// <summary>
            /// Retrieve the entity with the specified id, given the entityType
            /// </summary>
            /// <param name="entityName">"Category"</param>
            /// <param name="id">"1" Id of a specific entity.</param>
            /// <returns></returns>
            public BoolMessageItem Get(ScaffoldContext ctx)
            {
                BoolMessageItem creationResult = CreateContext(ctx, true, false, false);
                if (!creationResult.Success) return creationResult;

                EntityServiceContext entityActionContext = creationResult.Item as EntityServiceContext;

                EntityManager entitySvc = new EntityManager();
                BoolMessageItem result = entitySvc.Get(entityActionContext);
                return result;
            }


            /// <summary>
            /// Update the specific entity with property values specified.
            /// </summary>
            /// <param name="entityName">"Category"</param>
            /// <param name="propValues">List of key value pairs.
            /// Where the key is the property name, and the value is a string 
            /// representation of the property value.</param>
            /// <param name="id">"2"</param>
            public BoolMessage Update(ScaffoldContext ctx)
            {
                BoolMessageItem creationResult = CreateContext(ctx, true, true, true);
                if (!creationResult.Success) return creationResult;

                EntityServiceContext entityActionContext = creationResult.Item as EntityServiceContext;

                // Create the entity using the entity service.
                EntityManager service = new EntityManager();
                BoolMessage result = service.Update(entityActionContext);
                return result;
            }


            /// <summary>
            /// Delete the specific entity with the id.
            /// </summary>
            /// <param name="entityName">"Category"</param>
            /// <param name="id">"2"</param>
            public BoolMessage Delete(ScaffoldContext ctx)
            {
                BoolMessageItem creationResult = CreateContext(ctx, true, false, false);
                if (!creationResult.Success) return creationResult;

                EntityServiceContext entityActionContext = creationResult.Item as EntityServiceContext;

                // Create the entity using the entity service.
                EntityManager service = new EntityManager();
                BoolMessage result = service.Delete(entityActionContext);
                return result;
            }
            #endregion


            /// <summary>
            /// Get all the properties of the entity.
            /// </summary>
            /// <param name="entityName">The name of any of the supported entities that 
            /// are manageable.</param>
            /// <example>Given "Category" Returns :
            /// 1. Name
            /// 2. Id 
            /// 3. Description. etc.</example>
            /// <returns></returns>
            public IList<PropertyInfo> GetProperties(string entityName)
            {
                // Validate.
                if (string.IsNullOrEmpty(entityName)) { return null; }
                if (!EntityRegistration.ContainsEntity(entityName)) { return null; }

                Type typ = Type.GetType(entityName);
                IList<PropertyInfo> props = ReflectionUtils.GetWritableProperties(typ, null);
                return props;
            }


            private BoolMessageItem Validate(ScaffoldContext ctx)
            {
                int entityId = 0;
                // Validate the entity id as string.
                if (string.IsNullOrEmpty(ctx.EntityId)) return new BoolMessageItem(null, true, string.Empty);
                if (Int32.TryParse(ctx.EntityId, out entityId)) return new BoolMessageItem(entityId, true, string.Empty);

                string error = "Unable to parse entity id : " + ctx.EntityId;
                ctx.Errors.Add(error);
                return new BoolMessageItem(null, false, error);
            }


            private BoolMessageItem CreateContext(ScaffoldContext scaffoldContext, bool parseEntityId, bool createEntity, bool transferScaffoldUIValuesToEntity)
            {
                int entityId = 0;
                // Convert the strin entity id to an integer entity id.
                if (parseEntityId)
                {
                    BoolMessageItem parseResult = Validate(scaffoldContext);
                    if (!parseResult.Success) return parseResult;

                    entityId = (int)parseResult.Item;
                }

                // The type of the entity.
                Type typ = Type.GetType(scaffoldContext.EntityName);
                object entity = createEntity ? Activator.CreateInstance(typ) : null;

                EntityServiceContext enCtx = new EntityServiceContext(typ, entityId, entity);
                enCtx.Errors = scaffoldContext.Errors;

                // Transfer the values from the Dynamically generated Scaffold UI to the entity.
                if (createEntity && transferScaffoldUIValuesToEntity)
                {
                    // Set the entity properties using the properties supplied.       
                    ReflectionUtils.SetProperties<object>(enCtx.Item, scaffoldContext.PropValues);
                }
                return new BoolMessageItem(enCtx, true, string.Empty);
            }
       
    }
}
