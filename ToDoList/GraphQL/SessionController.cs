using GraphQL.AspNet.Attributes;
using GraphQL.AspNet.Controllers;

namespace ToDoList.GraphQL
{
    [GraphRoute("session")]
    public class SessionController : GraphController
    {
        [Query("get")]
        public int get()
        {
            return (int)HttpContext.Session.GetInt32("CurrentDatabase")!;
        }
        [Mutation("set")]
        public int set(int id)
        {
            HttpContext.Session.SetInt32("CurrentDatabase", id);
            return id;
        }
    }
}
