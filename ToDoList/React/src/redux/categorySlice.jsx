import { createSlice } from "@reduxjs/toolkit";

const categorySlice = createSlice({
    name: "categories",
    initialState: [],
    reducers: {
        addCategory: (state, action) => {
            const newCategory = {
                id: Date.now(),
                name: action.payload.name,
            };
            state.push(newCategory);
        },

        deleteCategory: (state, action) => {
            return state.filter((category) => category.id !== action.payload.id);
        },
    }
});

export const { 
    addCategory,
    deleteCategory
} = categorySlice.actions;

export default categorySlice.reducer;