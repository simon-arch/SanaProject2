﻿namespace ToDoList.Services
{
    public class ServiceFactory
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IHttpContextAccessor _contextAccessor;
        public enum CurrentDatabase
        {
            SQL,
            XML
        }
        public ServiceFactory(IServiceProvider serviceProvider, IHttpContextAccessor contextAccessor)
        {
            _serviceProvider = serviceProvider;
            _contextAccessor = contextAccessor;
        }
        public INoteService? GetNoteService()
        {
            IEnumerable<INoteService> services = _serviceProvider.GetServices<INoteService>();
            return _contextAccessor.HttpContext!.Session.GetInt32("CurrentDatabase") switch
            {
                0 => services.Single(s => s.GetType() == typeof(NoteServiceSQL)),
                _ => services.Single(s => s.GetType() == typeof(NoteServiceXML))
            };
        }
        public INoteService? GetNoteService(int serviceID)
        {
            IEnumerable<INoteService> services = _serviceProvider.GetServices<INoteService>();
            return serviceID switch
            {
                0 => services.Single(s => s.GetType() == typeof(NoteServiceSQL)),
                _ => services.Single(s => s.GetType() == typeof(NoteServiceXML))
            };
        }
        public ICategoryService? GetCategoryService()
        {
            IEnumerable<ICategoryService> services = _serviceProvider.GetServices<ICategoryService>();
            return _contextAccessor.HttpContext!.Session.GetInt32("CurrentDatabase") switch
            {
                0 => services.Single(s => s.GetType() == typeof(CategoryServiceSQL)),
                _ => services.Single(s => s.GetType() == typeof(CategoryServiceXML))
            };
        }
        public ICategoryService? GetCategoryService(int serviceID)
        {
            IEnumerable<ICategoryService> services = _serviceProvider.GetServices<ICategoryService>();
            return serviceID switch
            {
                0 => services.Single(s => s.GetType() == typeof(CategoryServiceSQL)),
                _ => services.Single(s => s.GetType() == typeof(CategoryServiceXML))
            };
        }
    }
}
