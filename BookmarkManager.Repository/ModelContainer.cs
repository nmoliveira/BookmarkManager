using BookmarkManager.Repository;
using Microsoft.Practices.Unity;

namespace BookmarkManager.Repository
{
    public static class ModelContainer
    {
        private static IUnityContainer _Instance;

        static ModelContainer()
        {
            _Instance = new UnityContainer();
        }

        public static IUnityContainer Instance
        {
            get
            {
                _Instance.RegisterType<IBookmarkRepository, BookmarkRepository>(new HierarchicalLifetimeManager());
                _Instance.RegisterType<ITagRepository, TagRepository>(new HierarchicalLifetimeManager());
                return _Instance;
            }
        }
    }
}
