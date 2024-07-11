import { createSlice } from '@reduxjs/toolkit';

const todoSlice = createSlice({
    name: 'todos',
    initialState: [],
    currentDatabase: 'SQL',
    reducers: {
        setTodos: (state, action) => {
            state.splice(0, state.length, ...action.payload.reverse().map((data) => ({
                id: data.id,
                name: data.name,
                description: data.description,
                created: data.created,
                modified: data.modified,
                deadline: data.deadline,
                completed: data.statuscode,
                categoriesNotes: data.categoriesNotes.map(obj => obj.categoryid),
            })));
            state.sort((a, b) => a.completed - b.completed);
        },

        addTodo: (state, action) => {
            const newTodo = {
                id: action.payload.id,
                name: action.payload.name,
                completed: false,
                description: action.payload.description,
                created: Date.now(),
                modified: Date.now(),
                deadline: action.payload.deadline !== '' ? action.payload.deadline : null,
                categoriesNotes: action.payload.categoriesNotes
            };
            state.unshift(newTodo);
            state.sort((a, b) => a.completed - b.completed);
        },

        finishTodo: (state, action) => {
            const index = state.findIndex (
                (todo) => todo.id === action.payload.id
            );
            state[index].completed = !state[index].completed;
            state[index].modified = Date.now();
            state.sort((a, b) => a.completed - b.completed);
        },

        deleteTodo: (state, action) => {
            return state.filter((todo) => todo.id !== action.payload.id);
        },

        removeCategory: (state, action) => {
            const index = state.findIndex (
                (todo) => todo.id === action.payload.id
            );
            const target = state[index].categoriesNotes.indexOf(action.payload.categoryId);
            target > -1 && state[index].categoriesNotes.splice(target, 1);
        },

        updateId: (state, action) => {
            const index = state.findIndex (
                (todo) => todo.id === action.payload.oldId
            );
            state[index].id = action.payload.newId
        }
    }
});

export const { 
    setTodos,
    addTodo,
    finishTodo,
    deleteTodo,
    removeCategory,
    updateId
} = todoSlice.actions;

export default todoSlice.reducer;