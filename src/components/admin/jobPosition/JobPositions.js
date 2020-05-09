import React, { Component } from "react";
import { Link, Route } from "react-router-dom";
import AddJobPosition from "./AddJobPosition";

class JobPositions extends Component{
    render() {
        return(
            <div className="card text-justify">
                <div className='card-header'>
                    Job Positions
                </div>
                <div className="card-body">
                    <Link to='/admin/job-positions/add' className='btn btn-primary'>Add job position</Link>
                    <Route path='/admin/job-positions/add' component={AddJobPosition} />
                </div>
            </div>
        );
    }
}

export default JobPositions;