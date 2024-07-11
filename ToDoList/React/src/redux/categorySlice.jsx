import { createSlice } from '@reduxjs/toolkit';

const categorySlice = createSlice({
    name: 'categories',
    initialState: [],
    reducers: {
        setCategories: (state, action) => {
            state.splice(0, state.length, ...action.payload.map((data) => ({
                id: data.id,
                name: data.name
            })));
        },

        addCategory: (state, action) => {
            const newCategory = {
                id: action.payload.id,
                name: action.payload.name
            };
            state.push(newCategory);
        },

        deleteCategory: (state, action) => {
            return state.filter((category) => category.id !== action.payload.id);
        },

        updateId: (state, action) => {
            const index = state.findIndex (
                (category) => category.id === action.payload.oldId
            );
            state[index].id = action.payload.newId
        }
    }
});

export const {
    setCategories,
    addCategory,
    deleteCategory,
    updateId
} = categorySlice.actions;

export default categorySlice.reducer;