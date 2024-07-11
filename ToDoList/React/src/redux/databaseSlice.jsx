import { createSlice } from '@reduxjs/toolkit';

const databaseSlice = createSlice({
    name: 'database',
    initialState: 'SQL',
    reducers: {
        setCurrentDatabase: (state, action) => {
            return state = action.payload;
        },
    },
});

export const { setCurrentDatabase } = databaseSlice.actions;

export default databaseSlice.reducer;