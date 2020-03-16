import React, { Component } from "react";
import { Link, Route } from "react-router-dom";
import AddDepartment from "./AddDepartment";

class Departments extends Component{
    render() {
        return(
            <div className="card text-justify">
                <div className='card-header'>
                    Departments
                </div>
                <div className="card-body">
                    <Link to='/admin/departments/add' className='btn btn-primary'>Add department</Link>
                    <Route path='/admin/departments/add' component={AddDepartment} />
                </div>
            </div>
        );
    }
}

export default Departments;