import React from 'react';
import { useSelector } from 'react-redux';
import CategoryItem from './CategoryItem';

const CategoryList = () => {
	const categories = useSelector((state) => state.categories);

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