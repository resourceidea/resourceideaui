import React, { Component } from "react";
import { Link, Route } from "react-router-dom";
import AddEmployee from "./AddEmployee";

class Employee extends Component{
    render() {
        return(
            <div className="card text-justify">
                <div className='card-header'>
                    Employees
                </div>
                <div className="card-body">
                    <Link to='/admin/employees/add' className='btn btn-primary'>Add employee</Link>
                    <Route path='/admin/employees/add' component={AddEmployee} />
                </div>
            </div>
        );
    }
}

export default Employee;