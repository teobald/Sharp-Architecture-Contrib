namespace SharpArchContrib.Specifications
{
    using System.Collections.Generic;
    using System.Linq;

    using Machine.Specifications;

    using NHibernate.Tool.hbm2ddl;

    using SharpArch.NHibernate;
    using SharpArch.NHibernate.Contracts.Repositories;
    using SharpArch.Testing.NHibernate;

    using SharpArchContrib.Data.NHibernate;
    using SharpArchContrib.Specifications.DomainModel;
    using SharpArchContrib.Specifications.DomainModel.Entities;

    public class specification_for_repository_search
    {
        protected static INHibernateRepository<Blog> subject;
        
        protected static IList<Blog> result;

        Establish context = () =>
            {
                ServiceLocatorHelper.InitialiseServiceLocator();
                new DefaultSessionFactoryKeyProvider().AddToServiceLocator<ISessionFactoryKeyProvider>();
                InitializeNHibernate();
                subject = new NHibernateRepository<Blog>();
            };

        protected static void InitializeNHibernate()
        {
            var configFile = "sqlite-nhibernate-config.xml";
            var mappingAssemblies = new string[] { "SharpArchContrib.Specifications" };
            var cfg = NHibernateSession.Init(
                new SimpleSessionStorage(),
                mappingAssemblies,
                new AutoPersistenceModelGenerator().Generate(),
                configFile);
            var schemaExport = new SchemaExport(cfg);
            schemaExport.Execute(true, true, false, NHibernateSession.Current.Connection, null);
        }

        protected static Blog GetTransientBlog()
        {
            var post1 = new Post
            {
                Title = "Test post 1",
                Body = "First blog post in test blog."
            };
            var post2 = new Post
            {
                Title = "Test post 2",
                Body = "Maybe I should write a real blog post about this."
            };
            var blog = new Blog
            {
                Name = "Someone's blog",
                Description = "Blog used for testing NHSearch functionality",
                Posts = new List<Post> { post1, post2 }
            };
            return blog;
        }
    }

    [Subject(typeof(NHibernateRepository<Blog>))]
    public class When_the_repository_is_searched_for_a_blog_by_existing_post_title : specification_for_repository_search
    {
        Establish context = () =>
        {
            subject.SaveOrUpdate(GetTransientBlog());
            subject.DbContext.CommitChanges();
        };
        
        Because of = () => result = subject.Search("Posts.Title:post 2");

        It should_return_a_result = () => result.ShouldNotBeEmpty();

        It should_return_the_correct_blog_post = () => result.First().Name.ShouldEqual("Someone's blog");

        Cleanup all = () => RepositoryTestsHelper.Shutdown();
    }
}