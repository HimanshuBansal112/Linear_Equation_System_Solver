cmake_minimum_required(VERSION 3.20)

project(LinearEquationSolver)

set(CMAKE_CXX_STANDARD 20)

include_directories(${CMAKE_SOURCE_DIR})

file(GLOB SOURCES "*.cpp")

add_executable(LinearEquationSolver ${SOURCES})