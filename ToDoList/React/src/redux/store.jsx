import { configureStore } from '@reduxjs/toolkit';
import todoReducer from './todoSlice';
import categoryReducer from './categorySlice'
import databaseReducer from './databaseSlice';

import { createEpicMiddleware } from 'redux-observable';
import { addTodoEpic, finishTodoEpic, deleteTodoEpic } from '../api/todoEpics';
import { addCategoryEpic, deleteCategoryEpic } from '../api/categoryEpics';

const epicMiddleware = createEpicMiddleware();

const store = configureStore({
    reducer: {
        todos: todoReducer,
        categories: categoryReducer,
        database: databaseReducer
    },
    middleware: (getDefaultMiddleware) => getDefaultMiddleware().concat(epicMiddleware)
});

epicMiddleware.run(addTodoEpic);
epicMiddleware.run(finishTodoEpic);
epicMiddleware.run(deleteTodoEpic);

epicMiddleware.run(addCategoryEpic);
epicMiddleware.run(deleteCategoryEpic);

export default store;