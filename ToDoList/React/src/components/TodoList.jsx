import React, { useEffect } from 'react';
import TodoItem from './TodoItem';
import { useSelector, useDispatch} from 'react-redux';
import { setTodos } from '../redux/todoSlice';
import { getOptions } from '../api/options';

const TodoList = () => {
	const dispatch = useDispatch();
	const todos = useSelector((state) => state.todos);
	const currentDatabase = useSelector(state => state.database);

	useEffect(() => {
		fetch(getOptions().apiEndPoint, {
			method: 'POST',
			headers: {
			'Content-Type': 'application/json',
            'CurrentDatabase': currentDatabase
			},
			body: JSON.stringify({
				query: `
					query {
						notes_Q {
							getAll {
								id,
								name,
								description,
								created,
								modified,
								deadline,
								statuscode,
								categoriesNotes {
									categoryid
								}
							}
						}
					}
				`
			})
		})
		.then(response => response.json())
		.then(data => {
			dispatch(setTodos(data.data.notes_Q.getAll));
		})
		
	}, [currentDatabase]);

	return (
		<ul className='list-group'>
			{todos.map((todo) => (
				<TodoItem
					todo = {todo}
					key = {todo.id}
				/>
			))}
		</ul>
	);
};

export default TodoList;