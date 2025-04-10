import {
    ColumnDef,
    flexRender,
    getCoreRowModel,
    useReactTable,
    type PaginationState,
} from "@tanstack/react-table";

import {
    Table,
    TableBody,
    TableCell,
    TableHead,
    TableHeader,
    TableRow,
} from "@/components/ui/table";
import { type Dispatch, type SetStateAction } from 'react';
import { Pagination, PaginationContent, PaginationItem } from '@/components/ui/pagination';
import { Button } from '@/components/ui/button';
import { Select, SelectContent, SelectItem, SelectTrigger } from '@/components/ui/select';
import { SelectValue } from '@radix-ui/react-select';

type DataTableProps<TData, TValue> = {
    columns: ColumnDef<TData, TValue>[];
    data: TData[];
    pagination?: PaginationState;
    setPagination?: Dispatch<SetStateAction<PaginationState>>;
    pageCount?: number;
};

export function DataTable<TData, TValue>({
    columns,
    data,
    pagination,
    setPagination,
    pageCount,
}: DataTableProps<TData, TValue>) {

    const table = useReactTable({
        data,
        columns,
        debugTable: true,
        getCoreRowModel: getCoreRowModel(),
        onPaginationChange: setPagination,
        state: {
            pagination
        },
        pageCount: pageCount
    });

    return (
        <div className="container mx-auto py-10">
            <div className='flex items-center'>
                <h2>

                </h2>
                {pagination ?
                    <Pagination className='flex items-center justify-end py-2'>
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
                                <span className="flex items-center gap-1 mx-2">
                                    <div>Page</div>
                                    <strong>
                                        {table.getState().pagination.pageIndex + 1} of{' '}
                                        {table.getPageCount().toLocaleString()}
                                    </strong>
                                </span>
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
                                    value={String(table.getState().pagination.pageIndex + 1)}
                                    onValueChange={value => {
                                        table.setPageSize(Number(value));
                                    }}
                                >
                                    <SelectTrigger>
                                        <SelectValue />
                                    </SelectTrigger>
                                    <SelectContent>
                                        {(Array.from({ length: table.getPageCount() }, (_, i) => i + 1)).map((pageCount) => {
                                            return (
                                                <SelectItem key={`pageSelect-${pageCount}`} value={String(pageCount)}>Go to page: {pageCount}</SelectItem>
                                            );
                                        })}
                                    </SelectContent>
                                </Select>
                            </PaginationItem>
                        </PaginationContent>
                    </Pagination>
                    : null}
            </div>
            {/* <div className="flex items-center justify-end space-x-2 py-4">





                <span className="flex items-center gap-1">
                    | Go to page:
                    <input
                        type="number"
                        min="1"
                        max={table.getPageCount()}
                        defaultValue={table.getState().pagination.pageIndex + 1}
                        onChange={e => {
                            const page = e.target.value ? Number(e.target.value) - 1 : 0;
                            table.setPageIndex(page);
                        }}
                        className="border p-1 rounded w-16"
                    />
                </span>
                
            </div> */}
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