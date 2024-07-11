import React, { useState } from 'react';
import { useDispatch } from 'react-redux';
import { setCurrentDatabase } from '../redux/databaseSlice';

const DatabaseForm = () => {
    const [database, setDatabase] = useState('0');
    const dispatch = useDispatch();

    const handleSubmit = (event) => {
        event.preventDefault();
        dispatch(setCurrentDatabase(database === '0' ? 'SQL' : 'XML'));
    };

	return (
        <div>
        <form onSubmit={handleSubmit}>
          <table className="table table-dark rounded" style={{verticalAlign: 'middle', margin: '0'}}>
            <thead>
              <tr>
                <th colSpan="2" style={{ textAlign: 'center', fontSize: '18px', fontWeight: '600' }}>
                  Database
                </th>
              </tr>
            </thead>
            <tbody>
              <tr>
                <td>
                  <input type="submit"
                  style={{width: '150px'}}
                  value="Change Database" 
                  className="btn btn-primary text-white" />
                </td>
                <td>
                  <select value={database}
                  id="database"
                  style={{float: 'left', width: '150px'}} 
                  onChange={(event) => setDatabase(event.target.value)}>
                    <option value="0">SQL</option>
                    <option value="1">XML</option>
                  </select>
                </td>
              </tr>
            </tbody>
          </table>
        </form>
      </div>
	);
};

export default DatabaseForm;