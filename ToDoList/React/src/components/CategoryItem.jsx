import React from 'react';
import { useDispatch } from 'react-redux';
import { deleteCategory } from '../redux/categorySlice';

const CategoryItem = ({ category }) => {
	const dispatch = useDispatch();

	const handleDeleteClick = () => {
		dispatch(deleteCategory({
			id: category.id
		}))
	};

	return (
        <tr key={category.id}>
			<td style={{maxWidth: '200px'}}>
				<p>{category.name}</p>
			</td>
			<td>
				<a className='btn btn-danger' 
				style={{float: 'right'}} 
				onClick={handleDeleteClick}>
				Delete</a>
			</td>
        </tr>
    )
};

export default CategoryItem;