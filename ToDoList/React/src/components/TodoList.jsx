import React from 'react';
import TodoItem from './TodoItem';
import { useSelector } from 'react-redux';

const TodoList = () => {
	const todos = useSelector((state) => state.todos);

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