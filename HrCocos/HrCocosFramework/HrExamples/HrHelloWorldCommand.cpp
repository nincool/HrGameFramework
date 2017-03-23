#include "HrExamples/HrHelloWorldCommand.h"
#include <iostream>

using namespace Hr;

void HrHelloWorldCommand::Trigger()
{

}

void HrHelloWorldCommand::Execute()
{
	std::cout << " HelloWorldCommand Execute! " << std::endl;
}
