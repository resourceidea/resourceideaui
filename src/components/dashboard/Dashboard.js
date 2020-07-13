import React, { Component } from "react";
import styled from "styled-components";
import { useSticky } from "react-table-sticky";
import {getDashboardColumns, getDashboardData} from "../../data/DashboardRepo";
import {getDashboardHeight, getDashboardWidth} from "../../utils/Dashboard";
import { useTable, useBlockLayout, useResizeColumns } from "react-table";
import {Link} from "react-router-dom";
import {capitalizeFirstLetter} from "../../utils/Common";

const data = getDashboardData();
const columns = getDashboardColumns()
const Table = ({ columns, data }) => {
    const {
        getTableProps,
        getTableBodyProps,
        headerGroups,
        rows,
        prepareRow
    } = useTable({
            columns,
            data
        },
        useBlockLayout,
        useResizeColumns,
        useSticky);

    const Styles = styled.div`
  padding: 1rem;
  .table {
    border: 1px solid #ddd;
    .tr {
      :last-child {
        .td {
          border-bottom: 0;
        }
      }
    }
    .th,
    .td {
      padding: 5px;
      border-bottom: 1px solid #ddd;
      border-right: 1px solid #ddd;
      background-color: #fff;
      overflow: hidden;
      :last-child {
        border-right: 0;
      }
      .resizer {
        display: inline-block;
        width: 5px;
        height: 100%;
        position: absolute;
        right: 0;
        top: 0;
        transform: translateX(50%);
        z-index: 1;
        &.isResizing {
          background: red;
        }
      }
    }
    &.sticky {
      overflow: scroll;
      .header,
      .footer {
        position: sticky;
        z-index: 1;
        width: fit-content;
      }
      .header {
        top: 0;
        box-shadow: 0px 3px 3px #ccc;
      }
      .footer {
        bottom: 0;
        box-shadow: 0px -3px 3px #ccc;
      }
      .body {
        position: relative;
        z-index: 0;
      }
      [data-sticky-td] {
        position: sticky;
      }
      [data-sticky-last-left-td] {
        box-shadow: 2px 0px 3px #ccc;
      }
      [data-sticky-first-right-td] {
        box-shadow: -2px 0px 3px #ccc;
      }
    }
  }
`;

    return (
        <Styles>
            <div {...getTableProps()} className='table sticky' style={{ width: getDashboardWidth(), height: getDashboardHeight() }}>
                <div className='header'>
                    {headerGroups.map(headerGroup => (
                        <div {...headerGroup.getHeaderGroupProps()} className='tr'>
                            {headerGroup.headers.map((column) => (
                                <div {...column.getHeaderProps()} className='th'>
                                    {column.render("Header")}
                                    <div
                                        {...column.getResizerProps()}
                                        className={`resizer ${column.isResizing ? 'isResizing' :  ''}`} />
                                </div>
                            ))}
                        </div>
                    ))}
                </div>
                <div {...getTableBodyProps()} className='body'>
                    {rows.map((row) => {
                        prepareRow(row);
                        return (
                            <div {...row.getRowProps()} className='tr'>
                                {row.cells.map((cell) => (
                                    <div {...cell.getCellProps()} className='td'>
                                        {cell.render("Cell")}
                                    </div>
                                ))}
                            </div>
                        );
                    })}
                </div>
            </div>
        </Styles>
    );
};

class Dashboard extends Component {
    render() {
        return (
            <div>
                <br />
                <div className='container-fluid'>
                <nav className='navbar navbar-expand-sm navbar-light bg-light border border-secondary'>
                    <ul className='navbar-nav mr-auto'>
                        <li>
                            <div className="btn-group">
                                <button className="btn btn-secondary btn-sm dropdown-toggle" type="button"
                                        data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                                    Change view
                                </button>
                                <div className="dropdown-menu">
                                    <a className="dropdown-item" href="#">Resources</a>
                                </div>
                            </div>&nbsp;&nbsp;
                        </li>
                        <li className='nav-item'>
                            <a href='#' className='btn btn-light btn-sm'>Add resource</a>&nbsp;&nbsp;
                        </li>
                        <li className='nav-item'>
                            <a href='#' className='btn btn-light btn-sm'>Assign task</a>
                        </li>
                    </ul>
                </nav>
                <hr style={{ marginTop: '10px', marginBottom: 0 }} />
                </div>
                <Table columns={columns} data={data} />
            </div>
        );
    }
}

export default Dashboard;