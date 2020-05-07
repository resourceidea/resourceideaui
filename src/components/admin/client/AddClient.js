import React, { Component } from "react";
import { Link } from "react-router-dom";

class AddClient extends Component {
    render() {
        return (
            <div className="text-left">
                <Link
                    to='/admin/clients'
                    className='btn btn-light btn-sm text-left'>Dashboard</Link>
                <Link
                    to='/admin/clients/list'
                    className='btn btn-light btn-sm text-left'>Clients list</Link>
                <br /><br />
                <h5>New client</h5>
                <hr />
                <form>
                    <div className='card'>
                        <div className='card-body bg-light'>
                            <div className='row'>
                                <div className='col-6'>
                                    <div className='form-group'>
                                        <label for='clientName'>Name</label>
                                        <input type='text' className='form-control' id='clientName' placeholder='Client name' />
                                    </div>
                                </div>
                                <div className='col-6'>
                                    <label for='name'>Industry</label>
                                    <input
                                        type='text'
                                        className='form-control'
                                        id='name'
                                        placeholder='Client industry' />
                                </div>
                            </div>
                            <div className='row'>
                                <div className='col-12'>
                                    <div className='form-group'>
                                        <label for='address'>Address</label>
                                        <input type='text' className='form-control' id='address' placeholder='Client address' />
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

export default AddClient;