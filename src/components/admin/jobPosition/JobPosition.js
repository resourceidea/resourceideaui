import React, { Component } from "react";
import AddJobPosition from "./AddJobPosition";
import { Link, Route } from "react-router-dom";

class JobPosition extends Component{
    render(){
        return(
            <div className="card text-justify">
                <div className="card-header">
                    Job positions
                </div>
                <div className="card-body">
                    <Link to='/admin/job-positions/add' className="btn btn-primary">New job position</Link>
                    <Route path="/admin/job-positions/add" component={AddJobPosition} />
                </div>
            </div>
        );
    }
}

export default JobPosition;