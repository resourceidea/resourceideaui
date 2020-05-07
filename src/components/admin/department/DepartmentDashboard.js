import React, { Component } from "react";
import { Link } from "react-router-dom";

class DepartmentDashboard extends Component{
    render(){
        return(
            <div>
                <div className='text-left'>
                    <Link 
                        to='/admin/departments' 
                        className='btn btn-light btn-sm text-left'>Dashboard</Link>&nbsp;&nbsp;
                    <Link 
                        to='/admin/departments/list'
                        className='btn btn-light btn-sm text-left'>Departments list</Link>&nbsp;&nbsp;
                    <Link 
                        to='/admin/departments/add'
                        className='btn btn-light btn-sm text-left'>Add new department</Link>&nbsp;&nbsp;
                </div>
                <br />
                <div className='text-justify'>
                    <h5>Departments Dashboard</h5>
                    <hr />
                </div>
            </div>
        );
    }
}

export default DepartmentDashboard;