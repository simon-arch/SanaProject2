import React from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { finishTodo, deleteTodo } from '../redux/todoSlice';
import { getCurrentDate } from "../utility/CurrentDate";

const TodoItem = ({ todo }) => {
	const categories = useSelector((state) => state.categories);

	const dispatch = useDispatch();
	const handleCompleteClick = () => {
		dispatch(finishTodo({
			id: todo.id,
		}));
	};

	const handleDeleteClick = () => {
		dispatch(deleteTodo({
			id: todo.id
		}));
	};

	return (
		<div className='card p-1 m-3' style={{width: '500px', border: '2px solid black', borderRadius: '20px', backgroundColor: '#28282B'}}>
			<div className='card-body' style={{borderRadius: '18px', background: 'repeating-linear-gradient(45deg, #fafafa 0px, #fafafa 4px, #f0f0f0 2px, #f0f0f0 9px)'}}>
			<a className='btn btn-danger mb-3 rounded-pill' style={{float: 'right', border: '2px darkred dashed'}} onClick={handleDeleteClick}>🗑</a>
				{ todo.completed ? (
						<p className='btn btn-success mb-3 rounded-pill' style={{border: '2px darkgreen solid'}}>Finished</p>
					) : (
						<div>
							<a style={{fontSize: '30px', marginRight: '10px', textDecoration: 'none', cursor: 'pointer'}} onClick={handleCompleteClick}>✅</a>
							<p className='btn btn-warning mb-3 rounded-pill' style={{border: '2px orange solid'}}>In Progress</p>
						</div>
					)}

				{ todo.completed ? (
					<div>
						<h4>
							<del>{todo.name}</del>
						</h4>
						<p>
							<del>{todo.description}</del>
						</p>
					</div>
				) : (
					<div>
						<h4>{todo.name}</h4>
						<p>{todo.description}</p>
					</div>
				) }
				
				<p className='task-created m-0' style={{fontSize: '13px'}}>
					<span style={{float: 'right'}}><b>Created:</b> {getCurrentDate(todo.created)}</span><br/>
					<span style={{float: 'right'}}><b>Updated:</b> {getCurrentDate(todo.modified)}</span>
					{todo.deadline !== null && (
						<span style={{ float: 'left'}}><b>Deadline:</b> {getCurrentDate(todo.deadline)}</span>
					)}
				</p>
			</div>
			<div style={{display: 'block', margin: 'auto', textAlign: 'center'}}>
				{todo.categoriesNotes.map((id) => {
				const category = categories.find((c) => c.id === id);
				if (!category) {
					return (null);
				}
				return (
					<span key={category.id} 
					className='btn btn-dark my-1 rounded-pill border-light' 
					style={{ borderWidth: '2px', margin: '2px', maxWidth: '150px', whiteSpace: 'nowrap', overflow: 'hidden', textOverflow: 'ellipsis'}}>
						{category.name}
					</span>
				);
				})}
			</div>
		</div>
	);
};

export default TodoItem;