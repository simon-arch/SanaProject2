﻿using ToDoList.Models;

namespace ToDoList.Services
{
    public interface INoteService
    {
        IEnumerable<Note> GetAll();
        int Add(Note note);
        void Delete(int id);
        Note? Get(int id);
        void Update(Note note, int id);
    }
}
