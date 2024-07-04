import React, { useState } from 'react';
import { useDispatch } from 'react-redux';
import { addTodo } from '../redux/todoSlice';
import { getCurrentDate } from '../utility/CurrentDate';
import { useSelector } from 'react-redux';

const AddTodoForm = () => {
	const [name, setName] = useState('');
	const [description, setDescription] = useState('');
	const [deadline, setDeadline] = useState('');
	const [categories, setCategories] = useState([]);

	const handleCategoryChange = (event) => {
		const selected = Array.from(event.target.selectedOptions, (option) => option.value);
		setCategories(selected);
	};

	const dispatch = useDispatch();
	const allCategories = useSelector((state) => state.categories);

	const onSubmit = (event) => {
		event.preventDefault();
		dispatch(addTodo({
			name: name,
			description: description,
			deadline: getCurrentDate(deadline),
			categories: categories.map(id => parseInt(id))
		}))
		setName('');
		setDescription('');
		setDeadline('');
		setCategories([]);
	};

	return (
		<div className="card p-1 m-3 text-white" style={{width: '500px', border: '2px solid black', borderRadius: '20px', backgroundColor: '#28282B'}}>
			<div className="card-body">
				<form onSubmit={onSubmit}>
					<table style={{width: '100%'}}>
						<tbody>
							<tr>
								<td style={{width: '100px'}}>Name:</td>
								<td><input value={name} 
									onChange={(event) => setName(event.target.value)} 
									required 
									placeholder="Note Title" 
									style={{width: '98%'}} 
									className="rounded mb-1" 
									autoComplete="off" 
									type="text"/>
								</td>
							</tr>
							<tr>
								<td>Description:</td>
								<td><textarea value={description} 
									onChange={(event) => setDescription(event.target.value)} 
									placeholder="Optional Description" 
									style={{width: '98%'}} 
									className="rounded" 
									autoComplete="off"/>
								</td>
							</tr>
							<tr>
								<td>Category:</td>
								<td>
									<select name="categories" 
									multiple 
									style={{width: '98%'}}
									value={categories}
									onChange={handleCategoryChange}>
										{allCategories.map((category) => (
											<option key={category.id} value={category.id}>{category.name}</option>
										))}
									</select>
								</td>
							</tr>
							<tr>
								<td>
									<input type="submit" 
									value="Add Task" 
									className="btn btn-primary text-white"/>
									</td>
								<td>
									<input value={deadline} 
									onChange={(event) => setDeadline(event.target.value)} 
									name="deadline" 
									style={{width: '98%'}} 
									type="datetime-local"/>
								</td>
							</tr>
						</tbody>
					</table>
				</form>
			</div>
		</div>
	);
};

export default AddTodoForm;