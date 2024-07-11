import React, { useEffect } from 'react';
import { useSelector, useDispatch } from 'react-redux';
import CategoryItem from './CategoryItem';
import { setCategories } from '../redux/categorySlice';
import { getOptions } from '../api/options';

const CategoryList = () => {
	const dispatch = useDispatch();
	const categories = useSelector((state) => state.categories);
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
						categories_Q {
							getAll {
                                id,
                                name
							}
						}
					}
				`
			})
		})
		.then(response => response.json())
		.then(data => {
			dispatch(setCategories(data.data.categories_Q.getAll));
		})
		
	}, [currentDatabase]);

	return (
        <div>
            <table className="table table-dark rounded" style={{verticalAlign: 'middle', margin: '0'}}>
                <tbody>
                    <tr>
                        <td style={{textAlign: 'center', fontSize: '18px', fontWeight: '600'}} colSpan="2">Available categories</td>
                    </tr>
                    {categories.map((category) => (
                        <CategoryItem
                            category = {category}
                            key = {category.id}
                        />
                    ))}
                </tbody>
            </table>
        </div>
	);
};

export default CategoryList;