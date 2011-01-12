namespace SharpArchContrib.Specifications
{
    using System.Collections.Generic;
    using System.IO;

    using Machine.Specifications;
    using Machine.Specifications.AutoMocking.Rhino;

    using Rhino.Mocks;

    using SharpArch.Core.PersistenceSupport;
    using SharpArch.Data.NHibernate;
    using SharpArch.Testing.NHibernate;

    using SharpArchContrib.Data.NHibernate.Search;
    using SharpArchContrib.Specifications.DomainModel.Entities;

    [Subject(typeof(IndexBuilder<Post>))]
    public class When_the_create_index_method_is_called : Specification<IIndexBuilder<Post>, IndexBuilder<Post>>
    {
        static IRepositoryWithTypedId<Post, int> postRepository;

        private Establish context = () =>
            {
                InitializeNHibernate();
                postRepository = DependencyOf<IRepositoryWithTypedId<Post, int>>();
                postRepository.Stub(x => x.GetAll()).Return(new List<Post>());
            };

        private static void InitializeNHibernate()
        {
            var configFile = "sqlite-nhibernate-config.xml";
            NHibernateSession.Init(new SimpleSessionStorage(), new string[] { }, configFile);
        }

        Because of = () => subject.BuildSearchIndex();

        It should_create_the_index_directory = () => Directory.Exists("LuceneIndex/Post/").ShouldBeTrue();

        It should_ask_the_repository_for_all_posts = () => postRepository.AssertWasCalled(x => x.GetAll());

        Cleanup all = () => RepositoryTestsHelper.Shutdown();
    }
}
