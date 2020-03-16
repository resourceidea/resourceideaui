import React, { Component } from "react";
import { Link, Route } from "react-router-dom";
import AddPeople from "./AddEmployee";

class Employee extends Component{
    render() {
        return(
            <div className="card text-justify">
                <div className='card-header'>
                    People
                </div>
                <div className="card-body">
                    <Link to='/admin/employee/add' className='btn btn-primary'>Add employee</Link>
                    <Route path='/admin/employee/add' component={AddEmployee} />
                </div>
            </div>
        );
    }
}

export default Employee;