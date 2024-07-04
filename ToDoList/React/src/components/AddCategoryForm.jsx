import React, { useState } from 'react';
import { useDispatch } from 'react-redux';
import { addCategory } from '../redux/categorySlice';

const AddCategoryForm = () => {
	const [name, setName] = useState('');

	const dispatch = useDispatch();

	const onSubmit = (event) => {
		event.preventDefault();
		dispatch(addCategory({
			name: name
		}))
        setName('');
	};

	return (
        <form onSubmit={onSubmit}>
            <table className="table table-dark rounded" style={{verticalAlign: 'middle', margin: '0'}}>
                <tbody>
                    <tr>
                        <td>
                            <input type="submit"
                            value="Add Category"
                            className="btn btn-primary text-white"/>
                        </td>
                        <td>
                            <input required 
                            style={{float: 'left', width: '180px'}}
                            onChange={(event) => setName(event.target.value)}
                            placeholder="Category Name" 
                            className="rounded" 
                            autoComplete="off" 
                            name="input"
                            value={name}
                            type="text"/>
                        </td>
                    </tr>
                </tbody>
            </table>
        </form>
	);
};

export default AddCategoryForm;