import { configureStore } from "@reduxjs/toolkit";
import todoReducer from './todoSlice';
import categoryReducer from './categorySlice'

export default configureStore({
    reducer: {
        todos: todoReducer,
        categories: categoryReducer
    },
});