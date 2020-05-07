import React, { Component } from "react";
import { Link } from "react-router-dom";

class AddDepartment extends Component {
    render() {
        return (
            <div className="text-left">
                <Link
                    to='/admin/departments'
                    className='btn btn-light btn-sm text-left'>Dashboard</Link>
                <Link
                    to='/admin/departments/list'
                    className='btn btn-light btn-sm text-left'>Departments list</Link>
                <br /><br />
                <h5>New department</h5>
                <hr />
                <form>
                    <div className='card'>
                        <div className='card-body bg-light'>
                            <div className='row'>
                                <div className='col-6'>
                                    <div className='form-group'>
                                        <label for='departmentName'>Name</label>
                                        <input type='text' className='form-control' id='departmentName' placeholder='Department name' />
                                    </div>
                                </div>
                            </div>
                            <input type='submit' value='Save' className='btn btn-danger btn-sm' />
                        </div>
                    </div>
                </form>
            </div>
        );
    }
}

export default AddDepartment;