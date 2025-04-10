import {
    ColumnDef,
    flexRender,
    getCoreRowModel,
    getPaginationRowModel,
    getSortedRowModel,
    useReactTable,
    type PaginationState,
    type SortingState,
} from "@tanstack/react-table";

import {
    Table,
    TableBody,
    TableCell,
    TableHead,
    TableHeader,
    TableRow,
} from "@/components/ui/table";
import { useState, type Dispatch, type SetStateAction } from 'react';
import { Pagination, PaginationContent, PaginationItem } from '@/components/ui/pagination';
import { Button } from '@/components/ui/button';
import { Select, SelectContent, SelectItem, SelectTrigger } from '@/components/ui/select';
import { SelectValue } from '@radix-ui/react-select';

type DataTableProps<TData, TValue> = {
    columns: ColumnDef<TData, TValue>[];
    data: TData[];
    tableTitle: string;
    pagination?: PaginationState;
    setPagination?: Dispatch<SetStateAction<PaginationState>>;
    pageCount?: number;
    sorting?: SortingState;
    setSorting?: Dispatch<SetStateAction<SortingState>>;
};

export function DataTable<TData, TValue>({
    columns,
    data,
    tableTitle,
    pagination,
    setPagination,
    pageCount,
    sorting,
    setSorting
}: DataTableProps<TData, TValue>) {

    const table = useReactTable({
        data,
        columns,
        debugTable: true,
        getCoreRowModel: getCoreRowModel(),
        manualPagination: true,
        onPaginationChange: setPagination,
        getSortedRowModel: getSortedRowModel(),
        onSortingChange: setSorting,
        state: {
            pagination,
            sorting
        },
        pageCount: pageCount,
    });

    return (
        <div className="container mx-auto py-10">
            <div className='flex items-center py-2'>
                <h2 className='text-4xl'>
                    {tableTitle}
                </h2>
                {pagination ?
                    <Pagination className='flex items-center justify-end'>
                        <PaginationContent>
                            <PaginationItem>
                                <Button
                                    variant="outline"
                                    size="sm"
                                    onClick={() => table.firstPage()}
                                    disabled={!table.getCanPreviousPage()}
                                >
                                    {'<<'}
                                </Button>
                            </PaginationItem>
                            <PaginationItem>
                                <Button
                                    variant="outline"
                                    size="sm"
                                    onClick={() => table.previousPage()}
                                    disabled={!table.getCanPreviousPage()}
                                >
                                    {'<'}
                                </Button>
                            </PaginationItem>
                            <PaginationItem>
                                <span className="flex items-center gap-1 mx-2">
                                    <div>Page</div>
                                    <strong>
                                        {table.getState().pagination.pageIndex + 1} of{' '}
                                        {table.getPageCount().toLocaleString()}
                                    </strong>
                                </span>
                            </PaginationItem>
                            <PaginationItem>
                                <Button
                                    variant="outline"
                                    size="sm"
                                    onClick={() => table.nextPage()}
                                    disabled={!table.getCanNextPage()}
                                >
                                    {'>'}
                                </Button>
                            </PaginationItem>
                            <PaginationItem>
                                <Button
                                    variant="outline"
                                    size="sm"
                                    onClick={() => table.lastPage()}
                                    disabled={!table.getCanNextPage()}
                                >
                                    {'>>'}
                                </Button>
                            </PaginationItem>

                            <PaginationItem>
                                <Select
                                    value={String(table.getState().pagination.pageSize)}
                                    onValueChange={value => {
                                        table.setPageSize(Number(value));
                                    }}
                                >
                                    <SelectTrigger>
                                        <SelectValue />
                                    </SelectTrigger>
                                    <SelectContent>
                                        {[10, 20, 30, 40, 50, 60, 70, 80, 90, 100].map(pageSize => (
                                            <SelectItem key={`pageSize-${pageSize}`} value={String(pageSize)}>Show {pageSize}</SelectItem>
                                        ))}
                                    </SelectContent>
                                </Select>
                            </PaginationItem>
                            <PaginationItem>
                                <Select
                                    value={String(table.getState().pagination.pageIndex)}
                                    onValueChange={value => {
                                        table.setPageIndex(Number(value));
                                    }}
                                >
                                    <SelectTrigger>
                                        <SelectValue />
                                    </SelectTrigger>
                                    <SelectContent>
                                        {(Array.from({ length: table.getPageCount() }, (_, i) => i)).map((pageNum) => {
                                            return (
                                                <SelectItem
                                                    key={`pageSelect-${pageNum}`}
                                                    value={String(pageNum)}
                                                >
                                                    Go to page: {pageNum + 1}</SelectItem>
                                            );
                                        })}
                                    </SelectContent>
                                </Select>
                            </PaginationItem>
                        </PaginationContent>
                    </Pagination>
                    : null}
            </div>
            <div className="rounded-md border overflow-auto">
                <Table>
                    <TableHeader className='bg-sidebar'>
                        {table.getHeaderGroups().map((headerGroup) => (
                            <TableRow key={headerGroup.id}>
                                {headerGroup.headers.map((header) => {
                                    return (
                                        <TableHead key={header.id}>
                                            {header.isPlaceholder
                                                ? null
                                                : flexRender(
                                                    header.column.columnDef.header,
                                                    header.getContext()
                                                )}
                                        </TableHead>
                                    );
                                })}
                            </TableRow>
                        ))}
                    </TableHeader>
                    <TableBody>
                        {table.getRowModel().rows?.length ? (
                            table.getRowModel().rows.map((row) => (
                                <TableRow
                                    key={row.id}
                                    data-state={row.getIsSelected() && "selected"}
                                >
                                    {row.getVisibleCells().map((cell) => (
                                        <TableCell key={cell.id}>
                                            {flexRender(cell.column.columnDef.cell, cell.getContext())}
                                        </TableCell>
                                    ))}
                                </TableRow>
                            ))
                        ) : (
                            <TableRow>
                                <TableCell colSpan={columns.length} className="h-24 text-center">
                                    No results.
                                </TableCell>
                            </TableRow>
                        )}
                    </TableBody>
                </Table>
            </div>
        </div>
    );
}