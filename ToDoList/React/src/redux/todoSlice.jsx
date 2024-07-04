import { createSlice } from "@reduxjs/toolkit";
import { getCurrentDate } from "../utility/CurrentDate";

const todoSlice = createSlice({
    name: "todos",
    initialState: [],
    reducers: {
        addTodo: (state, action) => {
            const newTodo = {
                id: Date.now(),
                name: action.payload.name,
                completed: false,
                description: action.payload.description,
                created: getCurrentDate(),
                modified: getCurrentDate(),
                deadline: action.payload.deadline,
                categories: action.payload.categories
            };
            state.unshift(newTodo);
            state.sort((a, b) => a.completed - b.completed);
        },

        finishTodo: (state, action) => {
            const index = state.findIndex (
                (todo) => todo.id === action.payload.id
            );
            state[index].completed = action.payload.completed;
            state[index].modified = getCurrentDate();
            state.sort((a, b) => a.completed - b.completed);
        },

        deleteTodo: (state, action) => {
            return state.filter((todo) => todo.id !== action.payload.id);
        },

        removeCategory: (state, action) => {
            const index = state.findIndex (
                (todo) => todo.id === action.payload.id
            );
            const target = state[index].categories.indexOf(action.payload.categoryId);
            target > -1 && state[index].categories.splice(target, 1);
        }
    }
});

export const { 
    addTodo,
    finishTodo,
    deleteTodo,
    removeCategory
} = todoSlice.actions;

export default todoSlice.reducer;