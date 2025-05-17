#include <iostream>
#include <unordered_map>

#include "main.h"

// Main function
int main() {
    GaussEliminationClass test;

    auto array = TakeInputMatrix(test);

    std::unordered_map<int, Fraction> answer = Solve(test, array);

    for (auto& [Key, Value] : answer) {
        std::cout << "x" << Key << " = " << Value.ToString() << std::endl;
    }

    std::cin.ignore(std::numeric_limits<std::streamsize>::max(), '\n');
    std::cin.get();

    return 0;
}