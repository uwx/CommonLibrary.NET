using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Security;

using NUnit.Framework;

using ComLib;
using ComLib.Account;
using ComLib.Entities;
using ComLib.Data;
using ComLib.ValidationSupport;
using ComLib.Entities.Extensions;
using ComLib.Authentication;
using ComLib.Feeds;


namespace CommonLibrary.Tests
{
    class Blog : ActiveRecordBaseEntity<Blog>, IEntity, IPublishable
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public bool IsPublished { get; set; }
        public bool IsPublic { get; set; }
        public string UrlAbsolute { get; set; }
        public string UrlRelative { get; set; }
        public string Author { get { return CreateUser; } }
        public string GuidId { get { return "abcdefghijklmnopqrst"; } }


        protected override IValidator GetValidatorInternal()
        {
            var val = new EntityValidator((validationEvent) =>
            {
                int initialErrorCount = validationEvent.Results.Count;
                IValidationResults results = validationEvent.Results;
                Blog entity = (Blog)validationEvent.Target;
                Validation.IsStringLengthMatch(entity.Title, false, true, true, 1, 10, results, "Title");
                Validation.IsStringLengthMatch(entity.Description, true, false, true, -1, 20, results, "Description");
                Validation.IsStringLengthMatch(entity.Content, false, false, false, -1, -1, results, "Content");
                return initialErrorCount == validationEvent.Results.Count;
            });
            return val;
        }
    }


    [TestFixture]
    public class EntityTests
    {

        [SetUp]
        public void Setup()
        {
            var repo = new RepositoryInMemory<Blog>();
            Blog.Init(() => repo, false);
        }


        [Test]
        public void CanValidate()
        {
            var blog = new Blog();
            blog.Title = "1234567890123";
            blog.Description = "123456789012345678901";
            blog.Content = "title and description exceed max lengths";

            IValidationResults results = new ValidationResults();
            // 1. The errors are available on the entity.
            bool isvalid = blog.Validate();            
            Assert.IsFalse(blog.Errors.IsValid);
            Assert.AreEqual(blog.Errors.Count, 2);
            blog.Errors.Clear();

            // 2. The errors are available on the entity.
            bool isvalid2 = blog.Validate();
            Assert.IsFalse(blog.Errors.IsValid);
            Assert.AreEqual(blog.Errors.Count, 2);

            // 3. This collects into different result set.
            // Force only 1 error to distinguist error result groups.
            blog.Title = "12345678";
            bool isvalid3 = blog.Validate(results);
            Assert.IsFalse(results.IsValid);
            Assert.AreEqual(results.Count, 1);

            // 4. Also check that the original errors didn't get changed.
            Assert.IsFalse(blog.Errors.IsValid);
            Assert.AreEqual(blog.Errors.Count, 2);

            Assert.IsFalse(isvalid);
            Assert.IsFalse(isvalid2);
            Assert.IsFalse(isvalid3);
        }


        [Test]
        public void CanDeleteOthersEntryWithoutSettings()
        {
            Auth.Init(new AuthWin("admin", new UserPrincipal(1, "kishore", "normaluser", "windows", true)));
            
            var blog = new Blog();
            blog.Title = "title";
            blog.Description = "description";
            blog.Content = "content";
            blog.Save();

            Auth.Init(new AuthWin("admin", new UserPrincipal(1, "john", "normaluser", "windows", true)));
            Blog.Delete(blog.Id);
            var blog2 = Blog.Get(blog.Id);
            Assert.IsNull(blog2);
        }


        [Test]
        public void CanNotDeleteOthersEntryWithoutSettings()
        {
            var repo = new RepositoryInMemory<Blog>();
            Blog.Init(() => repo, new EntitySettings<Blog>(){ EnableAuthentication = true }, false);
            Auth.Init(new AuthWin("admin", new UserPrincipal(1, "kishore", "normaluser", "windows", true)));

            var blog = new Blog();
            blog.Title = "title";
            blog.Description = "description";
            blog.Content = "content";
            blog.Save();

            Auth.Init(new AuthWin("admin", new UserPrincipal(1, "john", "normaluser", "windows", true)));
            Blog.Delete(blog.Id);
            var blog2 = Blog.Get(blog.Id);
            Assert.IsNotNull(blog2);
        }


        [Test]
        public void CanNotUpdateOthersEntryWithoutSettings()
        {
            var repo = new RepositoryInMemory<Blog>();
            Blog.Init(() => repo, new EntitySettings<Blog>() { EnableAuthentication = true }, false);
            Auth.Init(new AuthWin("admin", new UserPrincipal(1, "kishore", "normaluser", "windows", true)));

            var blog = new Blog();
            blog.Title = "title";
            blog.Description = "description";
            blog.Content = "content";
            blog.Save();

            Auth.Init(new AuthWin("admin", new UserPrincipal(1, "john", "normaluser", "windows", true)));
            blog.Title = "title2";
            blog.Content = "content2";
            blog.Save();
            var blog2 = Blog.Get(blog.Id);

            // Check that the update date has not changed which mean, the service didn't update the last change.
            Assert.AreEqual(blog2.UpdateDate, blog.UpdateDate);
        }


        [Test]
        public void CanCheckErrorsWithoutValidating()
        {
            var blog = new Blog();
            blog.Title = "1234567890123";
            blog.Description = "123456789012345678901";
            blog.Content = "title and description exceed max lengths";

            Assert.AreEqual(blog.Errors.Count, 0);
        }


        [Test]
        public void CanCreate()
        {
            Auth.Init(new AuthWin("admin", new UserPrincipal(1, "kishore", "admin", "windows", true)));

            var blog = new Blog();
            blog.Title = "1234567890";
            blog.Description = "12345678901234567890";
            blog.Content = "title and description";

            var service = EntityRegistration.GetService<Blog>();
            service.Create(blog);
            var blog2 = service.Get(blog.Id);
            Assert.AreEqual(blog2, blog);
        }


        [Test]
        public void CanSetAuditData()
        {
            Auth.Init(new AuthWin("admin", new UserPrincipal(1, "kishore", "admin", "windows", true)));
            
            var blog = new Blog();
            blog.Title = "1234567890123";
            blog.Description = "123456789012345678901";
            blog.Content = "title and description exceed max lengths";

            var service = EntityRegistration.GetService<Blog>();
            service.Create(blog);
            Assert.AreEqual(blog.CreateUser, "kishore");
            Assert.AreEqual(blog.UpdateUser, "kishore");
            Assert.AreEqual(blog.CreateDate.Date, DateTime.Today);
        }

    }



    [TestFixture]
    public class EntityExtensionTests
    {
        [Test]
        public void CanSetAuditData()
        {
            Auth.Init(new AuthWin("admin", new UserPrincipal(1, "kishore", "admin", "windows", true)));
            Blog blog = new Blog();
            blog.AuditAll();

            Assert.AreEqual(blog.CreateUser, "kishore");
            Assert.AreEqual(blog.UpdateUser, "kishore");
            Assert.AreEqual(blog.CreateDate.Date, DateTime.Today.Date);
            Assert.AreEqual(blog.UpdateDate.Date, DateTime.Today.Date);
        }


        [Test]
        public void CanCopy()
        {
            Auth.Init(new AuthWin("admin", new UserPrincipal(1, "kishore", "admin", "windows", true)));
            Blog blog = new Blog();
            blog.AuditAll();
            Assert.IsTrue(blog.IsCopyable());
        }


        [Test]
        public void CanNotCopy()
        {
            Auth.Init(new AuthWin("admin", new UserPrincipal(1, "kishore", "admin", "windows", true)));
            Blog blog = new Blog();
            blog.AuditAll();
            
            // Now change the user.
            Auth.Init(new AuthWin("admin", new UserPrincipal(1, "rob", "normal_user", "windows", true)));
            Assert.IsFalse(blog.IsCopyable());
        }


        [Test]
        public void CanCheckOwner()
        {
            Auth.Init(new AuthWin("admin", new UserPrincipal(1, "kishore", "admin", "windows", true)));
            Blog blog = new Blog();
            blog.AuditAll();

            Assert.IsTrue(blog.IsOwnerOrAdmin());
        }


        [Test]
        public void CanUpdateModelSafely()
        {
            Auth.Init(new AuthWin("admin", new UserPrincipal(1, "kishore", "admin", "windows", true)));
            Blog blog = new Blog();
            blog.AuditAll();
            blog.Title = "title";
            blog.Content = "content";
            blog.Description = "description";

            blog.DoUpdateModel<Blog>(ent =>
            {
                blog.Title = "title_updated";
                blog.Content = "content_updated";
                blog.Description = "description_updated";
        
                // Now attempt to update audit fields ( which should not be allowed )
                // The DoUpdateModel will reset them.
                ent.CreateUser = "new user";
                ent.UpdateUser = "new user";
                ent.CreateDate = DateTime.Now.AddDays(-4);
                ent.UpdateDate = DateTime.Now.AddDays(-3);
                ent.UpdateComment = "other comment";
            });

            Assert.AreEqual(blog.Title, "title_updated");
            Assert.AreEqual(blog.Content, "content_updated");
            Assert.AreEqual(blog.Description, "description_updated");
            Assert.AreEqual(blog.CreateUser, "kishore");
            Assert.AreEqual(blog.UpdateUser, "kishore");
            Assert.AreEqual(blog.CreateDate.Date, DateTime.Today.Date);
            Assert.AreEqual(blog.UpdateDate.Date, DateTime.Today.Date);
        }


        [Test]
        public void CanServeToAuthorEvenIfNotPublished()
        {
            Auth.Init(new AuthWin("admin", new UserPrincipal(1, "kishore", "writer", "windows", true)));
            Blog blog = new Blog();
            blog.AuditAll();
            blog.IsPublic = false;
            blog.IsPublished = false;

            Assert.IsTrue(blog.IsOkToServe());
        }


        [Test]
        public void CanNotServeIfNotPublished()
        {
            Auth.Init(new AuthWin("admin", new UserPrincipal(1, "kishore", "writer", "windows", true)));
            Blog blog = new Blog();
            blog.AuditAll();
            blog.IsPublic = false;
            blog.IsPublished = false;
            
            // Change user.
            Auth.Init(new AuthWin("admin", new UserPrincipal(1, "john", "writer", "windows", true)));

            Assert.IsFalse(blog.IsOkToServe());
        }
    }
}
