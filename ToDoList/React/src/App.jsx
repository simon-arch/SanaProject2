import React from 'react';
import AddTodoForm from './components/AddTodoForm';
import TodoList from './components/TodoList';
import CategoryList from './components/CategoryList';
import AddCategoryForm from './components/AddCategoryForm'
import DatabaseForm from './components/DatabaseForm';
import 'bootstrap/dist/css/bootstrap.min.css';

const App = () => {
	return (
		<div className="container">
			<div className="row">
				<div className="col-lg-11">
					<div className="d-flex justify-content-center">
						<div>
							<AddTodoForm />
							<TodoList />
						</div>
					</div>
				</div>
				<div className="col-lg-1">
					<div style={{ float: 'right', width: '370px'}}>
						<div className="card m-3 text-white" style={{ border: '2px solid black', backgroundColor: '#28242C'}}>
							<DatabaseForm />
							<AddCategoryForm />
							<CategoryList />
						</div>
					</div>
				</div>
			</div>
		</div>
	);
};

export default App;